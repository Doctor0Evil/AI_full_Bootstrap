use aws_lambda_events::event::cloudwatch_events::CloudWatchEvent;
use aws_sdk_secretsmanager::Client as SecretsClient;
use aws_sdk_sns::Client as SnsClient;
use couchbase::{Cluster, ClusterOptions, PasswordAuthenticator};
use lambda_runtime::{run, service_fn, Error as LambdaError, LambdaEvent};
use log::{error, info, warn};
use serde::{Deserialize, Serialize};
use serde_json::{json, Value};
use std::env;

// Constants from configuration
const CB_URL: &str = "https://cb.1ij7wycqsmzoxe.cloud.couchbase.com:18091";
const CB_USERNAME: &str = "databaseuser";
const CB_PASSWORD_SECRET: &str = "arn:aws:secretsmanager:us-east-1:123456789012:secret:agentic-credentials-abc123";
const SNS_TOPIC_ARN: &str = "arn:aws:sns:us-east-1:123456789012:AgenticStackAlerts";

#[derive(Serialize, Deserialize)]
struct Response {
    status_code: i32,
    body: String,
}

#[derive(Serialize, Deserialize)]
struct RouteResult {
    status: String,
    action: Option<String>,
    reason: Option<String>,
}

async fn get_couchbase_password(secrets_client: &SecretsClient) -> Result<String, LambdaError> {
    let response = secrets_client
        .get_secret_value()
        .secret_id(CB_PASSWORD_SECRET)
        .send()
        .await
        .map_err(|e| {
            error!("Failed to retrieve secret: {}", e);
            LambdaError::from(format!("SecretsManager error: {}", e))
        })?;

    let secret: Value = serde_json::from_str(
        response
            .secret_string()
            .ok_or_else(|| {
                error!("No secret string found");
                LambdaError::from("No secret string found")
            })?,
    )?;

    let password = secret["databasepassword"]
        .as_str()
        .ok_or_else(|| {
            error!("Invalid secret format");
            LambdaError::from("Invalid secret format")
        })?
        .to_string();

    Ok(password)
}

async fn connect_to_couchbase() -> Result<Cluster, LambdaError> {
    let secrets_client = SecretsClient::new(&aws_config::load_from_env().await);
    let password = get_couchbase_password(&secrets_client).await?;
    let auth = PasswordAuthenticator::new(CB_USERNAME, &password);
    let options = ClusterOptions::new(auth);

    let cluster = Cluster::connect(CB_URL, options)
        .await
        .map_err(|e| {
            error!("Couchbase connection failed: {}", e);
            LambdaError::from(format!("Couchbase error: {}", e))
        })?;

    cluster.bucket("Home").await.map_err(|e| {
        error!("Failed to connect to Home bucket: {}", e);
        LambdaError::from(format!("Bucket error: {}", e))
    })?;

    info!("Connected to Couchbase");
    Ok(cluster)
}

async fn route_task(sns_client: &SnsClient, data: &Value) -> RouteResult {
    let change_type = data["event_type"]
        .as_str()
        .unwrap_or("unknown")
        .to_string();

    if change_type == "data_mutation" {
        let message = json!({
            "event": "data_mutation",
            "data": data,
            "timestamp": data["timestamp"].as_str().unwrap_or("")
        });

        match sns_client
            .publish()
            .topic_arn(SNS_TOPIC_ARN)
            .message(message.to_string())
            .subject("Couchbase Data Change")
            .send()
            .await
        {
            Ok(_) => {
                info!("Routed data_mutation event: {}", data["id"].as_str().unwrap_or("unknown"));
                RouteResult {
                    status: "success".to_string(),
                    action: Some("routed_to_sns".to_string()),
                    reason: None,
                }
            }
            Err(e) => {
                error!("SNS publish failed: {}", e);
                RouteResult {
                    status: "error".to_string(),
                    action: None,
                    reason: Some(format!("SNS error: {}", e)),
                }
            }
        }
    } else {
        warn!("Unsupported event type: {}", change_type);
        RouteResult {
            status: "skipped".to_string(),
            action: None,
            reason: Some("unsupported_event_type".to_string()),
        }
    }
}

async fn handler(event: LambdaEvent<CloudWatchEvent>) -> Result<Response, LambdaError> {
    info!("Received event: {}", serde_json::to_string(&event)?);

    let cluster = connect_to_couchbase().await?;
    let sns_client = SnsClient::new(&aws_config::load_from_env().await);

    let records = event
        .payload
        .detail
        .map(|detail| vec![detail])
        .unwrap_or_else(|| vec![json!({})]);

    let mut results = Vec::new();
    for record in records {
        let result = route_task(&sns_client, &record).await;
        results.push(result);
    }

    info!("Processed {} records", results.len());
    Ok(Response {
        status_code: 200,
        body: json!({
            "status": "success",
            "results": results
        })
        .to_string(),
    })
}

#[tokio::main]
async fn main() -> Result<(), LambdaError> {
    env_logger::init();
    run(service_fn(handler)).await
}
