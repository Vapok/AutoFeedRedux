# AutoFeedRedux by Vapok

**AutoFeedRedux** is a lightweight, high-performance take on automatic feeding for Valheim. It eliminates excessive container looping and world tracking overhead by letting tameable creatures smartly locate and feed from nearby player-built containers when hungry.

---

## Features

* 🚶 **Direct-to-Container Navigation**: When hungry, tameable animals navigate directly to the nearest container containing compatible food.
* 📦 **Clean Inventory Consumption**: Food is consumed straight from the container inventory once the animal arrives—no dropped items littering the ground.
* 🛡️ **Container Protection**: Protects feed chests and troughs from being damaged or destroyed by creatures during the taming process.
* ⚡ **Optimized Performance**: Highly efficient spatial queries that avoid scanning every container across the entire world.
* 🌐 **Server-Synced Configuration**: Fully integrated with Jotunn configuration syncing to enforce server-side settings on multiplayer servers.

---

## Configuration Settings

All settings can be customized in-game or via the BepInEx configuration file (`vapok.mods.AutoFeedRedux.cfg`).

| Setting | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| **Enable Auto Feeder** | `bool` | `true` | Enables or disables automatic feeding from nearby containers. |
| **Feed Range in Meters** | `float` | `30` | Maximum search distance (in meters) from the animal to find valid feed containers. |
| **Require Move to Feed** | `bool` | `true` | When enabled, animals will walk to the container to feed. When disabled, animals feed instantly on the spot if within range. |
| **Move Proximity** | `float` | `1` | The distance (in meters) from the container the animal must reach before feeding. |
| **Protect Feed Containers** | `bool` | `true` | Prevents creatures from damaging containers designated as feed troughs. |
| **Disallow Feed** | `string` | `""` | Comma-separated list of item names to exclude from auto-feeding (e.g. `Carrot, Turnip`). |
| **Disallow Animal** | `string` | `""` | Comma-separated list of creature names to exclude from auto-feeding (e.g. `Boar, Lox`). |

---

### Mod Author Details

![Vapok Gaming](https://avatars.githubusercontent.com/u/1264136?s=180&v=4)

Author: [Vapok](https://github.com/Vapok)

Source: [GitHub](https://github.com/Vapok/AutoFeedRedux)

Discord: [Vapok's Mod Community](https://discord.gg/5YAJkRFBXt)

Patch Notes: [GitHub Changelog](https://github.com/Vapok/AutoFeedRedux/blob/main/CHANGELOG.md)
