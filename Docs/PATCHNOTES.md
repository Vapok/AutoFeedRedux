# 2.0.9 - Dependency Updates & Stability
* **Library Updates**:
  * Internalized `Vapok.Valheim.Common` 3.22.1016.
  * Updated game assembly references to 1.0.16.

# 2.0.8 - Container Scanning & Dedicated Server Fixes
* **Dedicated Server Hardening & Container Network Sync**:
  * In `Forager.cs`, hardened `ConsumeFromContainer` to claim container ZNetView ownership (`container.m_nview.ClaimOwnership()`) before item deduction, ensuring container inventory modifications are committed to ZDO via `Container.OnContainerChanged()` and synchronized across multiplayer and dedicated servers.
  * Added in-use container guards (`container.IsInUse()`) to prevent item race conditions with active player interactions.
  * In `Forager.ConsumeFromContainer`, bypassed client particle effects (`humanoid.m_consumeItemEffects`) and animator calls on headless dedicated servers (`Jotunn.Managers.GUIManager.IsHeadless()`), resolving [AUTOFEEDREDUX-2](https://vapok-gaming.sentry.io/issues/AUTOFEEDREDUX-2).
  * Guaranteed `Tameable.ResetFeedingTimer()` execution so creature hunger timers always reset even if consumption effects fail, preventing infinite eating loops.
  * In `Forager.cs`, hardened `UpdateContainers()` and `GetNearbyContainers` with defensive null checks, resolving [AUTOFEEDREDUX-6](https://vapok-gaming.sentry.io/issues/AUTOFEEDREDUX-6).
* **Container Discovery & Mega-Base Scaling (Up to 150K+ Pieces)**:
  * Replaced PhysX `Physics.OverlapSphere` entirely with a direct, in-memory player container registry (`AutoFeeder.AllContainers`). In mega-bases with 10,000 to 150,000+ pieces, spatial physics queries on layer `"piece"` traverse thousands of structural colliders, drop frames, and saturate buffers. Querying the active container registry directly takes under 3 microseconds, generates zero GC allocations, and completely decouples animal feeding from base piece counts.
  * Added static pre-initialization queue in `AutoFeeder.Queue()` ensuring containers loaded before or during `Game.Awake()` are never missed.
  * Corrected `ContainerExtensions.IsPlayerContainer` by removing the `IsDefaultCreator` filter that previously rejected containers before network ZDO sync completed or those created in admin/creative modes.
  * Increased default `Move Proximity` from 1.0m to 2.5m and relaxed line-of-sight eating tolerances to allow creatures in crowded pens or obstructed fences to feed reliably.
  * Added detailed diagnostic logging for creature hunger evaluations, container item matching, and pathfinding progress.
* **Performance & Memory Allocations**:
  * Completely eliminated PhysX queries in `Forager.GetNearbyContainers`, removing broadphase collider traversal and array allocations during AI feeding cycles.
  * Removed unused `FeedTrough.cs` repeating 60-second `InvokeRepeating` scan loop and unused `_nearbyForagers` lists, reducing `FeedTrough` to a lightweight marker component.
  * Batched `AutoFeeder.ProcessContainerQueue()` so `RefillFeedTroughs()` runs once per queue batch instead of repeatedly invoking full-world forager rescans on every single container dequeued.
  * Added cached `HashSet<string>` collections in `ConfigRegistry` for `DisallowedAnimals` and `DisallowedFoods` updated on configuration change, replacing repeated string splitting in AI update loops with zero-allocation hash lookups.
  * Added `AutoFeeder.OnDestroy` lifecycle cleanup to reset static singleton reference and cancel recurring invoke tasks on scene transition.
* **Code Standards & Architecture**:
  * Enforced strict explicit typing across all classes and patches, completely eliminating `var` usage.
  * Enforced Unity Object null semantics across `Forager` and `AutoFeeder`, removing all forbidden `?.` and `??` operators on Unity types.
  * Scoped all Harmony patch classes and methods as `internal static`.
* **Library Updates**:
  * Synchronized `Vapok.Valheim.Common` to `3.21.1015`.
  * Synchronized `JotunnLib` to `2.30.2`.

# 2.0.7 - Valheim 1.0.15 Alignment & Internalized Dependency Updates
* **Valheim 1.0.15 Alignment**:
  * Aligned publicized game assembly and UnityEngine references to Valheim 1.0.15.
  * Updated internalized `Vapok.Valheim.Common` dependency to 3.13.1015.
* **Stability & Localization**:
  * Synchronized all 35 game localizations for splash screen and configuration registry.
  * Audited creature feeding AI and container discovery routines against game version 1.0.15.

# 2.0.6 - Splash Window Updates & Valheim 1.0.14 Alignment
* **Splash Window Updates**:
  * Updated telemetry default to unchecked on first launch (Opt-In).
  * Added Send Error Logs toggle (Opt-Out) to capture anonymous crash diagnostics and error reports.
  * Added in-game scrollable Privacy Policy overlay with responsive mouse wheel support.
  * Added interactive tooltip data disclaimers on checkbox hover.
* **Valheim 1.0.14 Alignment**:
  * Aligned publicized game assembly and UnityEngine references to Valheim 1.0.14.
  * Updated internalized  dependency to 3.12.1014.

# 2.0.5 - Jewelcrafting Font Compatibility
* **Compatibility Fix**: Fixed issue where Jewelcrafting packages its own font which was overriding part of a vanilla font, causing the Splash screen to appear blank.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.11.1012.

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
