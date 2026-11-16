# AEROGATE

*A 3D quadcopter flight training and free-roam simulation through a neon-lit futuristic skyline.*

---

## Overview

**AeroGate** is a 3D quadcopter flight simulation built in Unity 6 using the Universal Render Pipeline (URP) for our Game Development course at the **Cambodia Academy of Digital Technology (CADT)**. Pilots master precision quadcopter handling through a 9-gate aerial obstacle course set between neon-accented skyscrapers before unlocking open-airspace Free Flight mode.

---

## Objective and Rules

* **Training Mode (9-Gate Course):** Pilot the quadcopter from the launch pad through all **9 sequential checkpoint rings** and touch down safely on the **Finish Helipad (`FinishPad`)** to complete the course.
* **Best Time & Unlock System:** Completing the training course records your fastest run via `PlayerPrefs` and unlocks **Free Flight Mode**.
* **Free Flight Mode:** Explore the neon city skyline freely with no gate timers or route restrictions.
* **Collision & Safety Rules:**
  * Colliding with skyscrapers, obstacles, or the ground triggers **Drone Crashed** in both Training Mode and Free Flight Mode.
  * Touching down on designated landing zones (`SafePad` or `FinishPad`) registers a safe landing.
  * Pressing **`R`** immediately resets the drone and restarts the current mode.

---

## Training Course Progression

| Stage | Segment | Flight Focus & Maneuvers |
| :--- | :--- | :--- |
| **Stage 0** | Boot & Telemetry | System initialization, HUD check, and mode selection (`Training` / `Free Flight`) |
| **Stage 1** | Gates 1–2 | Vertical takeoff, hover stabilization, and initial forward pitch alignment |
| **Stage 2** | Gates 3–5 | Narrow skyscraper canyon navigation, banking turns, and high-altitude climb |
| **Stage 3** | Gates 6–9 | Controlled descent, yaw realignment, and precision approach gates |
| **Stage 4** | Final Helipad | Speed bleed-off and upright touchdown extraction on `FinishPad` |
| **Bonus** | Free Flight | Unrestricted open-airspace exploration across the city skyline |

---

## Controls

| Key / Input | Action |
| :--- | :--- |
| **`W` / `S`** | Pitch forward / backward |
| **`A` / `D`** | Roll / bank left / right |
| **`Space`** | Ascend (increase throttle) |
| **`Left Ctrl`** | Descend (decrease throttle) |
| **`Q` / `E`** | Yaw (rotate left / right) |
| **`R`** | Restart immediately in current mode |
| **`Esc`** | Pause / Resume game |
| **On-Screen UI** | Select mode (`Training` / `Free Flight`), retry, or return to menu |

---

## Unity Version and Requirements

| Specification | Details |
| :--- | :--- |
| **Unity Editor Version** | Unity `6000.3.8f1` (Unity 6.3 LTS) |
| **Render Pipeline** | Universal Render Pipeline (URP) |
| **Main Scene** | `Assets/Scenes/Mainlevel.unity` |
| **Supported Platforms** | Windows (Intel 64-bit) & macOS (Universal: Apple Silicon & Intel) |
| **Input System** | Unity Input System / Keyboard & Mouse |

---

## GitHub Repository

[Go to GitHub Repository](https://github.com/VitouPorleak78/Aerogate.git)

---

## Setup and play in Unity

1. Install **Unity Hub** and Unity Editor **6000.3.8f1**. Include **Mac Build Support** for macOS builds or **Windows Build Support (Mono)** for Windows builds.
2. Clone the repository:

```bash
git clone https://github.com/VitouPorleak78/Aerogate.git
cd Aerogate
```

3. In Unity Hub, choose **Add project from disk** and select the cloned folder containing `Assets`, `Packages` and `ProjectSettings`.
4. Open the project with the specified Editor version and wait for assets and packages to finish importing.
5. Open `Assets/Scenes/Mainlevel.unity`.
6. Press **Play**, then select **Training** or **Free Flight**.
7. Follow the tutorial and controls above. Press **Esc** to pause or **R** to restart the run.

Open the existing scene directly; scene-generation tools are not required to play. Unity generates its local cache folders on first import.

---

## Playable build - MacOS

[Download AeroGate for macOS](https://github.com/VitouPorleak78/Aerogate/releases/download/v1.0/Aerogate.app.zip)

1. Download and extract the complete build archive.
2. Open `AeroGate.app` on macOS.
3. Select **Training** or **Free Flight** and play using a keyboard and mouse.

---

## Playable build - Windows 64-bit

[Download AeroGate for Windows](https://github.com/VitouPorleak78/Aerogate/releases/download/v1.0/Aerogate-Windows.zip)

1. Download the Windows ZIP once it has been attached to the release.
2. Use **Extract All** to extract the complete archive.
3. Open `Aerogate.exe` inside the extracted `AeroGate-Windows` folder.
4. Keep the executable and all accompanying files and folders together. Select **Training** or **Free Flight** to play.

Build files are ignored by Git and distributed separately through [GitHub Releases](https://github.com/VitouPorleak78/Aerogate.git).

---

## Gameplay video

[Watch the gameplay recording on YouTube](https://youtu.be/RqCbN8u2OUo)

---

## Team members and contributions

| Member Name | Role | Contributions |
| :--- | :--- | :--- |
| **Porleak Vitou** | Game UI & System Flow | HUD telemetry, start/pause/crash/completion menus, game loop state management, and build packaging |
| **Yan Sovanpisoth** | Obstacle & Level Design | 9-gate checkpoint progression, skyscraper obstacle layout, and collision/landing zone triggers |
| **Siv Kimleng** | Drone Physics & Controls | Quadcopter flight controller, pitch/roll/yaw/throttle physics, and camera follow mechanics |

---

## Asset Credits

* **3D Models & Environment:** Custom quadcopter drone rig, modular checkpoint ring prefabs, `LaunchPad` / `FinishPad` helipads, and city skyscraper blockout geometry.
* **Materials & Shaders:** Custom URP neon skyline materials (`Skyscraper`, `M_Window_Cyan`, `M_Rooftop`).
* **UI & Fonts:** Unity TextMeshPro (`TMP`) for flight telemetry HUD and menu interfaces.

---

## Tutorial References and Scripting Assistance

| Resource / Tool | Usage |
| :--- | :--- |
| **Unity 6 Documentation (URP & Input System)** | Scene lighting setup, Build Profiles configuration, and RigidBody flight physics reference |
| **Google Gemini** | Architecture planning, C# game manager/telemetry logic debugging, and build/documentation workflow |
| **Google Antigravity** | Project workspace inspection and C# script refactoring assistance |

---

## Project Status

* **Status:** Completed (`v1.0` Release)
* **Completed Milestones:**
  * 9-gate Training Course with `PlayerPrefs` Best Time tracking
  * Unlockable Free Flight mode & crash/restart state loop
  * Neon skyline URP environment & flight telemetry HUD
  * Standalone builds packaged for Windows 64-bit and macOS Universal