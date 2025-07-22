// Copyright [yyyy] [name of copyright owner]
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at:
//     https://www.apache.org/licenses/LICENSE-2.0 
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Additional Licenses:
// - LLVM Project: Dual licensed under UIUC "BSD-like" and MIT licenses
// - Third-party code in `third_party/` directories has their own LICENSE.txt
// - ONNX Runtime: MIT License
// - Engine.io-client: MIT License
// - nanoflann: BSD License
// - Unity Companion License v1.3 for specific components

fn main() {
    println!("Gnomovision version 69, Copyright (C) [year] [name of author]");
    println!("Gnomovision comes with ABSOLUTELY NO WARRANTY; for details type 'show w'");
    println!("This is free software, and you are welcome to redistribute it under certain conditions; type 'show c' for details.");

    loop {
        let mut input = String::new();
        std::io::stdin().read_line(&mut input).unwrap();
        match input.trim() {
            "show w" => show_warranty(),
            "show c" => show_conditions(),
            _ => println!("Unknown command. Use 'show w' or 'show c'"),
        }
    }
}

fn show_warranty() {
    println!("\nABSOLUTELY NO WARRANTY:");
    println!("THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND,");
    println!("EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF");
    println!("MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.");
    println!("IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,");
    println!("DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR");
    println!("OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE");
    println!("USE OR OTHER DEALINGS IN THE SOFTWARE.\n");
}

fn show_conditions() {
    println!("\nREDISTRIBUTION AND USE CONDITIONS:");
    println!("1. You may reproduce and distribute copies of the Work in any medium,");
    println!("   with or without modifications, in Source or Object form.");
    println!("2. You must give recipients a copy of this License.");
    println!("3. Modified files must carry prominent notices stating changes.");
    println!("4. Retain all original copyright, patent, trademark, and attribution notices.");
    println!("5. If combining with GPLv2 code, conflicts are resolved by retroactively");
    println!("   waiving Apache 2.0 patent/indemnity provisions for the Combined Software.");
    println!("6. Third-party code in separate directories must retain their own licenses.\n");
}

fn show_llvm_exceptions() {
    println!("\nLLVM EXCEPTIONS:");
    println!("Embedded portions in object code do not require Apache 2.0 Section 4(a), 4(b), or 4(d) compliance.");
    println!("Dual licensing applies to libc++ and libunwind under UIUC and MIT licenses.\n");
}

fn show_third_party_notices() {
    println!("\nTHIRD-PARTY NOTICES:");
    println!("- `third_party/unity_plugin_api`: Unity Companion License v1.3");
    println!("- `third_party/nanoflann`: BSD License");
    println!("- `third_party/onnxruntime`: MIT License");
    println!("- `third_party/engine.io-client`: MIT License\n");
}
