// SPDX-License-Identifier: MIT
pragma solidity ^0.8.24;

/// @title UiAssetRegistryStub - For tracking Bootstrap 5 artifact hashes/releases
contract UiAssetRegistryStub {
    struct Release {
        string cid_or_hash;
        uint256 timestamp;
    }
    mapping(uint256 => Release) public releases;
    uint256 public releaseCount;

    function publishRelease(string calldata cid_or_hash) external {
        releases[++releaseCount] = Release(cid_or_hash, block.timestamp);
    }

    function latestRelease() external view returns (Release memory) {
        return releases[releaseCount];
    }
}

.
├── .github/
│   └── workflows/
│       └── bootstrap-nexsm.yml
├── frontend/
│   ├── package.json
│   ├── vite.config.js       # Or webpack.config.js, etc.
│   └── src/
│       └── ...             # Your UI source using Bootstrap 5
├── public/
│   └── ...                 # Built UI/asset output
├── server/
│   └── ...                 # Simulation backend (optional)
└── ...
name: Build & Deploy Bootstrap v5 NEXSM Dashboard

on:
  push:
    branches: [ main ]
    paths-ignore:
      - '**.md'
      - '**/*.pdf'
  pull_request:
    branches: [ main ]
  workflow_dispatch:

jobs:
  build-bootstrap-ui:
    runs-on: ubuntu-latest
    defaults:
      run:
        working-directory: frontend

    steps:
      - name: Checkout source code
        uses: actions/checkout@v4

      - name: Set up Node.js (Latest LTS)
        uses: actions/setup-node@v4
        with:
          node-version: '20.x'
          cache: npm

      - name: Install dependencies
        run: npm ci

      - name: Build Bootstrap v5 assets (Vite/Webpack)
        run: npm run build

      - name: Verify built assets
        run: |
          ls -lh dist || ls -lh build || ls -lh ../public # adjust depending on your output

      - name: Upload artifact (built UI)
        uses: actions/upload-artifact@v4
        with:
          name: bootstrap5-nexsm-ui
          path: frontend/dist  # or ./build or wherever your output is

  # (Optional) preview or deploy steps follow
  # You can add deployment here, e.g., GitHub Pages, S3, Docker image, etc.
  # Example: Deploy to gh-pages
  deploy-gh-pages:
    if: github.ref == 'refs/heads/main'
    needs: build-bootstrap-ui
    runs-on: ubuntu-latest

    steps:
      - name: Download built artifact
        uses: actions/download-artifact@v4
        with:
          name: bootstrap5-nexsm-ui

      - name: Deploy to GitHub Pages
        uses: peaceiris/actions-gh-pages@v4
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
          publish_dir: ./dist
