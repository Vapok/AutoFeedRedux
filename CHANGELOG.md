# 2.0.2 - Unified Splash Screen & Telemetry Controls
* Added integration with unified Vapok.Common startup splash screen.
* Added player configuration controls for startup splash screen visibility and anonymous usage telemetry.

<details>
<summary><b>2.0 Changelog History (Valheim Release)</b> (<i>click to expand</i>)</summary>

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
