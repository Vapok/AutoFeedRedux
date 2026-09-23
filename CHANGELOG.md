# 2.0.8 - Container Scanning & Dedicated Server Fixes
* Fixed an issue where container searching could cause errors on dedicated servers.
* Improved safety and multiplayer synchronization when tames eat food directly from storage containers.
* Fixed an issue where animals in busy bases or fenced pens could have trouble finding food containers.
* Significantly reduced game lag and memory usage by optimizing animal feeding checks and container scanning.
* Updated Jotunn to 2.30.2 and internal dependencies for stability.

<details>
<summary><b>2.0 Changelog History (Valheim Release)</b> (<i>click to expand</i>)</summary>

### 2.0.7 - Valheim 1.0.15 Alignment & Internalized Dependency Updates
* **Valheim 1.0.15 Alignment**: Updated game assembly references and internalized `Vapok.Valheim.Common` 3.13.1015.
* **Localization & Stability**: Re-synchronized 35-language splash localizations and verified patch compatibility.

### 2.0.6 - Splash Window Updates & Valheim 1.0.14 Alignment
* **Splash Window Updates**:
  * Telemetry is now unchecked when first loaded (Opt-In visibility)
  * Added Send Error Logs (Opt-Out)
  * Privacy Policy is now available directly in-game
  * Added Data Disclaimers on hover over checkboxes for transparency on what data is sent
* **Valheim 1.0.14 Alignment**: Updated game assembly references and internalized `Vapok.Valheim.Common` 3.12.1014.


### 2.0.5 - Jewelcrafting Font Compatibility
* Fixed: Jewelcrafting packages it's own font which was overriding part of a vanilla font, causing the Splash screen to appear blank.
### 2.0.4 - Updated README with Telemetry Information
* Updated the README.md with Anonymous Telemetry information per request of mod stores.

### 2.0.3 - Unified Splash Screen & Telemetry Controls
* **Unified Startup Splash Screen**: Integrated with a centralized startup splash screen.
  * Added configurable `Show on Game Startup` which can be enabled or disabled in the configuration file.
* **Anonymous Telemetry**: 
  * Added configurable `Enable Anonymous Telemetry` configuration which can be enabled or disabled in the configuration file.
    * Defaults to enabled with auto-opt-in on launch. Uncheck to Opt-Out
    * ANONYMOUS DATA ONLY - I track version number and usage data. No personal data is ever collected. For more information, see the [Privacy Policy](https://vapok.io/privacy-policy/).

### 2.0.1 - Dependency & Compatibility Maintenance
* **Dependency Updates**: Updated Jotunn and BepInEx runtime package bindings.
* **Compatibility Maintenance**: Verified compatibility against the latest Valheim 1.0 release.
* **Documentation Improvements**: Standardized README, user guides, and technical patch documentation.

### 2.0.0 - Valheim 1.0 Update & Direct Container Feeding
* Updated for Valheim 1.0.
* Overhauled animal feeding navigation: tames now pathfind and walk directly to food containers when feeding.
* Food is now consumed directly from the container upon arrival.
* Updated default configuration values:
  * Auto Feeder enabled by default.
  * Feed Range increased to 30 meters.
  * Move Proximity updated to 1 meter.
* Added stability fixes and performance improvements.

</details>

<details>
<summary><b>1.0 Changelog History (Valheim Early Access)</b> (<i>click to expand</i>)</summary>

### 1.1.4 - Updated Dependencies
* Updated all dependencies to latest versions.

### 1.1.3 - Updated Dependencies
* Updated all dependencies to latest versions.

### 1.1.2 - LookingAt Warning Fix
* Updated looking direction detection to match recent Valheim API changes.

### 1.1.1 - Dedicated Server Config Syncing Fix
* Resolved an issue preventing dedicated servers from properly enforcing configuration settings on connected clients.
* Added graceful dependency handling and notifications.

### 1.1.0 - Jotunn Migration
* Updated for Valheim 0.221.4 and transitioned to Jotunn library.

### 1.0.4 - Valheim 0.217.28 Update
* Updated for Valheim 0.217.28.

### 1.0.3 - Valheim 0.217.24 Update
* Updated for Valheim 0.217.24.

### 1.0.2 - Logging Fixes
* Fixed non-critical error log messages occurring during creature spawn and death events.

### 1.0.1 - Container Protection & Bug Fixes
* Resolved an inventory extraction edge case.
* Added Container Protection logic to discourage tames from damaging nearby food containers while being tamed.

### 1.0.0 - Initial Release of AutoFeedRedux
* Initial release of automatic container feeding mechanics for tamed creatures.
* Added configurable feed range, movement proximity, container protection, and creature/food exclusion lists.

</details>
