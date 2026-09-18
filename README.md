<div align="center">

# 🥩 AutoFeedRedux

### *Automated creature feeding and container management for Valheim.*

[![GitHub Release](https://img.shields.io/github/v/release/Vapok/AutoFeedRedux?include_prereleases&logo=github&style=for-the-badge)](https://github.com/Vapok/AutoFeedRedux/releases)
[![Thunderstore Version](https://img.shields.io/thunderstore/v/Vapok/AutoFeedRedux?logo=thunderstore&style=for-the-badge)](https://thunderstore.io/c/valheim/p/Vapok/AutoFeedRedux/)
[![Nexus Mods](https://img.shields.io/badge/Nexus_Mods-Available-da8e35?logo=nexusmods&style=for-the-badge)](https://www.nexusmods.com/valheim/mods/3168)
<br>
[![Discord](https://img.shields.io/badge/Discord-Join%20Community-7289da?logo=discord&logoColor=white&style=for-the-badge)](https://discord.gg/5YAJkRFBXt)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

---

</div>

Tired of tossing stacks of berries, mushrooms, and meat onto the muddy pen floor only for half of it to despawn or get trampled? **AutoFeedRedux** automates the feeding routine for your tamed livestock and beasts. Tames will dynamically pathfind to nearby storage containers (chests, carts, barrels) and eat directly from them when hungry!

---

<div align="center">

<br>

[![Survival Servers](https://raw.githubusercontent.com/Vapok/AutoFeedRedux/main/images/survivalservers_banner.png)](https://www.survivalservers.com/services/game_servers/valheim/?ref=vapok)

</div>

## 🐾 How It Works

* 📦 **Container Feeding**: Place a storage container inside or near your animal pens and stock it with their favorite foods (carrots, turnips, berries, meats, etc.).
* 🧭 **Dynamic Pathfinding**: When an animal becomes hungry, it senses nearby food containers within range and pathfinds directly to the chest.
* 🍽️ **Direct Consumption**: Upon arriving within proximity of the container, the creature consumes the food directly from the container's inventory.
* 🛡️ **Container Protection**: While wild creatures are in the process of being tamed, they are discouraged from attacking the food containers they feed from.

---

## ⚙️ Configuration & Settings

All settings can be tailored in real-time using the in-game [BepInEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager) (default hotkey: <kbd>F1</kbd>) or via the configuration file.

| Setting | Default | Description |
| :--- | :--- | :--- |
| **Enable Auto Feeder** | `true` | Enables or disables the auto-feeding mechanics globally. |
| **Feed Range (Meters)** | `30.0m` | Maximum search radius in meters from the creature to detect food containers. |
| **Require Move to Feed** | `true` | When enabled, animals must physically walk to the container to eat. When disabled, animals eat remotely within range. |
| **Move Proximity** | `1.0m` | The stopping distance in meters from the container required for the animal to consume food. |
| **Protect Containers** | `true` | Discourages wild creatures in the taming process from attacking food containers. |
| **Disallow Feed** | *(Empty)* | Comma-separated list of item names to forbid creatures from eating (e.g. `Carrot, Turnip`). |
| **Disallow Animal** | *(Empty)* | Comma-separated list of creature names to exclude from auto-feeding (e.g. `Boar, Lox`). |

---

## 🛡️ Advanced Safeguards

* 🔒 **Anti-Despawn & Waste Prevention**: Eliminates the need to drop loose food items on the ground, keeping your pens clean and preventing item despawns.
* ⚡ **Optimized Performance**: Creature feeding searches are event-driven with zero per-frame update loops.
* 🌐 **ServerSync Enforced**: On dedicated servers, all configuration rules are securely managed by server administrators and synchronized to connected clients.

---

## 🤝 Verified Mod Compatibility

<div align="center">

| Mod | Compatibility Status |
| :--- | :--- |
| **Multi-User-Chests (MUC)** | 🟢 Fully Supported (Safe container inventory consumption) |
| **AdventureBackpacks** | 🟢 Fully Supported |
| **All Tameable / Custom Tame Mods** | 🟢 Fully Supported (Respects vanilla and modded tame food lists) |
| **Fast Item Transfer** | 🟢 Fully Supported |

</div>

---

## 🌐 Available Translations

<div align="center">

🇺🇸 **English** (Default)

</div>

*Want to help translate Auto Feed Redux? Community translations are welcome! Please submit a PR on [GitHub](https://github.com/Vapok/AutoFeedRedux) or stop by our [Discord](https://discord.gg/5YAJkRFBXt).*

---

## 📥 Installation & Server Setup

### Mod Manager (Recommended)
1. Install via **R2ModMan** or **Thunderstore Mod Manager**.
2. Dependencies (`BepInExPack`, `Jotunn (JVL)`) are installed automatically.

### Dedicated Server Requirements
* **Client & Server Required**: AutoFeedRedux must be installed on both the dedicated server and connecting clients.
* **ServerSync**: Configuration values set on the dedicated server will lock and synchronize to all connecting players.

---

## 🔒 Anonymous Telemetry, Error Reporting & Privacy

AutoFeedRedux includes lightweight, privacy-first telemetry and error reporting to help monitor mod stability, diagnose unhandled bugs, and track active version adoption across game updates.

* **100% Anonymous**: We never collect personal data, Steam IDs, IP addresses, character/world names, or file system paths. Stack traces from errors are automatically sanitized to strip local user directories.
* **Granular Player Control**:
  * **Anonymous Telemetry (Opt-In)**: Tracks version adoption and session launches. Defaults to **unchecked / disabled** when first loaded (`Enable Anonymous Telemetry = false`).
  * **Error Reporting (Opt-Out)**: Captures sanitized mod crash diagnostics to rapidly identify and fix bugs. Defaults to **enabled** (`Send Error Reports = true`) with one-click opt-out.
  * **Data Disclaimers**: Hover over any toggle in the startup modal for interactive tooltip disclaimers detailing exactly what data is transmitted.
* **In-Game & Online Privacy Policy**: The full privacy policy can be viewed directly in-game by clicking **`[ PRIVACY POLICY ]`** on the startup splash modal, or online at [vapok.io/privacy-policy](https://vapok.io/privacy-policy/).
* **Configuration Files**: Settings can be managed in-game via the startup modal, through the BepInEx Configuration Manager, or under `[Local Config]` in `BepInEx/config/vapok.mods.AutoFeedRedux.cfg`.

---

<div align="center">

### 👨‍💻 Created by Vapok Gaming

[![Vapok Gaming](https://avatars.githubusercontent.com/u/1264136?s=120&v=4)](https://github.com/Vapok)

**Author**: [Vapok](https://github.com/Vapok)  
**Source Code**: [GitHub Repository](https://github.com/Vapok/AutoFeedRedux)  
**Community & Support**: [Discord Server](https://discord.gg/5YAJkRFBXt)  
**Changelog**: [Release Notes](https://github.com/Vapok/AutoFeedRedux/blob/main/CHANGELOG.md)

</div>
