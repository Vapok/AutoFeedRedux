# 2.0.4 - Updated README with Telemetry Information
* **Documentation Update**: Updated the README.md with Anonymous Telemetry and Privacy section per request of mod stores.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.9.1012.

# 2.0.3 - Unified Splash Screen & Telemetry Controls
* **Unified Startup Splash Screen & Telemetry**:
  * Updated `Vapok.Valheim.Common` dependency reference to `v3.5.1012`.
  * Registered mod metadata with centralized `ModSplashManager`.
  * Added `ShowSplashOnStartup` and `Enable Anonymous Telemetry` configuration bindings to `ConfigRegistry`.

# 2.0.1 - Dependency & Compatibility Maintenance
* **Runtime & Dependency Updates**:
  * Synchronized package manifest and project references with Jotunn `2.30.0` and BepInEx `5.4.2350`.
  * Verified build pipeline and ILRepack bundling with `Vapok.Valheim.Common` `3.2.1012`.
* **Compatibility & Documentation**:
  * Validated container feeding routines and creature AI pathing against current Valheim 1.0 builds.
  * Standardized mod documentation, changelog tiers, and release staging.

# 2.0.0 - Valheim 1.0 Update & Direct Container Feeding
* **Valheim 1.0 Compatibility & Core Updates**:
  * Updated assembly references for Valheim 1.0 (`1.0.12`), BepInEx 5.4.2350, and Jotunn 2.30.0.
  * Rebuilt and retargeted for .NET Framework 4.8.
  * Integrated with `Vapok.Valheim.Common` 3.2.1012 for unified logging and configuration management.
* **Navigation & Feeding Overhaul (`Require Move to Feed`)**:
  * Refactored creature navigation logic: animals now dynamically pathfind directly to the target feeding container's transform position.
  * Food items are consumed directly from the container inventory on arrival instead of dropping items into the world beforehand.
  * Added validation checks to ensure feed item availability remains consistent between navigation start and container arrival.
* **ZDO Container Detection & State Synchronization**:
  * Replaced legacy ZDO lookup patterns with Valheim's standard `ZDOVars` keys for container detection.
  * Enhanced multiplayer network synchronization for container inventory item consumption.
* **Configuration Defaults**:
  * `Enable Auto Feeder` default set to `true`.
  * `Feed Range in Meters` default updated to `30m`.
  * `Move Proximity` default updated to `1m`.
* **Stability & Guardrails**:
  * Added defensive null checks across container lookup loops and tame state queries.
  * Prevented NREs during creature despawn and chunk unload events.

# 1.1.4 - Updated Dependencies
* Updated all package references and runtime dependencies to latest versions.

# 1.1.3 - Updated Dependencies
* Updated dependencies for compatibility with latest Valheim minor updates.

# 1.1.2 - LookingAt API Fix
* Updated method call from obsolete `LookingAt` to `LookingTowards` following vanilla Valheim API refactoring.

# 1.1.1 - Dedicated Server Config Syncing Fix
* Resolved regression preventing server configuration synchronization from enforcing client settings on dedicated servers.
* Added `BepInDependency` flags for graceful handling and error reporting when dependencies are missing.

# 1.1.0 - Jotunn Migration & ServerSync Transition
* Migrated from standalone ServerSync to Jotunn JVL configuration sync framework.
* Updated for Valheim 0.221.4.

# 1.0.4 - Update for Valheim 0.217.28
* Updated game references for Valheim 0.217.28 compatibility.

# 1.0.3 - Update for Valheim 0.217.24
* Updated game references for Valheim 0.217.24 compatibility.

# 1.0.2 - Spawn & Death Event Logging Cleanup
* Fixed non-critical error log messages occurring during creature spawn and death events.

# 1.0.1 - Container Protection & Inventory Fixes
* Fixed rare inventory desynchronization bug during container item extraction.
* Added Container Protection logic to discourage tames in the taming process from damaging food containers.

# 1.0.0 - Initial Release of AutoFeedRedux
* Initial release of automated creature feeding mechanics from nearby containers.
* Implemented `Feed Range`, `Require Move`, `Move Proximity`, `Disallow Feed`, and `Disallow Animal` configuration parameters.
