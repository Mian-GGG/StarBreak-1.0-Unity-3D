# 3D Space Shooter

A 3D space combat game developed using **Unity and C#**, featuring spaceship combat, enemy AI navigation, a scaled solar system, raycast-based weapons, and a seven-level progression system.

## Gameplay Preview

**Gameplay Video:** [Watch the gameplay on YouTube]([YOUTUBE_VIDEO_LINK](https://youtu.be/rix2rA14vE0))

## Overview

The 3D Space Shooter is a space combat game in which the player takes control of a spaceship commander from Earth's elite Space Force.

After being deployed on a mission in another galaxy, the commander is called back to Earth to respond to an alien invasion. Enemy spaceships have taken control of the planetary system and established hostile space stations around the planets.

The objective is to travel through the solar system, locate enemy space stations, eliminate hostile spaceships, and eventually clear the system of the invading forces.

The game features a large-scale solar system, spaceship combat, enemy AI, player health and shield systems, boost mechanics, and seven levels of progression.

## Gameplay Features

- **3D Spaceship Controls:** Control spaceship orientation using mouse movement and navigate through space.
- **Boost System:** Use a rechargeable boost mechanic to increase spaceship movement speed, supported by thruster and warp-drive particle effects.
- **Raycast-Based Combat:** Use raycasting for player and enemy weapon hit detection, with particle effects providing visual feedback.
- **Enemy AI:** Enemy spaceships use Unity's NavMesh navigation system to pursue the player.
- **Enemy Space Stations:** Space stations act as enemy spawning points and trigger encounters when approached.
- **Health and Shield Systems:** The player has separate health and shield resources, while enemies have individual health bars.
- **Waypoint Navigation:** An on-screen waypoint helps guide the player towards enemy space stations.
- **Seven-Level Progression:** Progress through multiple levels with increasing enemy counts and difficulty.
- **Persistent Gameplay Scenes:** Separate scenes handle the main menu, mission briefing, pause menu, level completion, death screen, and victory screen.

## Technical Implementation

### 1. Solar System Simulation

A scaled-down representation of the solar system was created using approximately accurate relative planetary distances.

The planets were scaled using a 1:10,000 ratio to make the solar system manageable within the game environment. The player spaceship was also scaled down accordingly.

Planet rotation and orbital movement were implemented to create a more immersive space environment.

### 2. Floating-Origin System

One of the main technical challenges was maintaining positional accuracy while allowing the player to travel across a large solar system.

As the player moved further away from the world origin, floating-point precision errors began affecting object positioning and rendering.

To address this, a floating-origin approach was implemented:

- Monitor the player's distance from the world origin.
- Once a specified distance is exceeded, move the planetary environment in the opposite direction.
- Reset the player's position back to the world origin.
- Adjust the Rigidbody position where applicable.

This allows the player to travel across the environment while keeping world coordinates relatively small and reducing floating-point precision issues.

### 3. Enemy AI and 3D Navigation

Unity's NavMesh and NavMeshAgent systems were used to handle enemy navigation and pursuit.

However, NavMesh navigation primarily operates on a baked navigation surface, which presented a challenge in a fully three-dimensional space environment.

A hybrid movement approach was implemented:

- An invisible navigation object follows the NavMeshAgent's calculated horizontal position.
- The enemy spaceship copies the navigation object's horizontal coordinates.
- Additional vertical movement logic adjusts the spaceship's height based on the player's position.

This allowed enemy spaceships to pursue the player in three dimensions while still benefiting from Unity's NavMesh pathfinding.

### 4. Raycast-Based Weapon System

Raycasting was used for weapon hit detection instead of relying entirely on instantiated physical projectiles.

The player and enemy spaceships use raycasts to detect hits and apply damage.

Particle systems provide the visual laser effects, separating the visual representation of the weapon from its hit-detection logic.

### 5. Player Systems

The player spaceship includes several gameplay systems:

- Mouse-based spaceship rotation.
- Forward and backward movement.
- Rechargeable speed boost.
- Separate health and shield resources.
- Enemy targeting and shooting.
- Waypoint navigation.
- Gameplay HUD displaying health, shields, boost, distance, and remaining enemies.

### 6. Scene Management and Pause System

Separate Unity scenes were used for different game states, including the start menu, mission briefing, pause menu, level completion, death, and victory screens.

The pause system uses additive scene loading to display the pause menu without unloading the current gameplay scene.

`Time.timeScale` is set to zero to pause gameplay, and the pause scene is unloaded when the player resumes.

Particle effects in the pause scene use unscaled time so that background visual effects can continue playing while the game is paused.

## Controls

| Input | Action |
|---|---|
| Mouse Movement | Rotate spaceship |
| W | Move forward |
| S | Move backward |
| Left Shift | Activate speed boost |
| Left Mouse Button | Fire weapons |
| Escape | Open pause menu |

## Technologies Used

- **Unity 3D** – Game engine and development environment.
- **C#** – Gameplay programming and game logic.
- **Unity NavMesh / AI Navigation** – Enemy navigation and pathfinding.
- **Physics Raycasting** – Weapon hit detection.
- **Unity Particle System** – Laser, thruster, warp-drive and explosion effects.
- **Unity Scene Management** – Level progression, menus and additive scene loading.
- **TextMeshPro** – In-game text and HUD elements.

## Source Code

This repository contains selected C# scripts from the project to demonstrate the gameplay systems and programming techniques used during development.

The scripts are organised around the following responsibilities:

| Script | Description |
|---|---|
| `Player.cs` | Player movement, combat, health, shields, boost and level progression. |
| `EnemyMovement.cs` | Enemy NavMesh navigation and player pursuit. |
| `EnemyMovCopier.cs` | Enemy positioning, shooting, damage and destruction. |
| `EnemySpawner.cs` | Enemy spawning and level-based enemy configuration. |
| `GameManager.cs` | Menu management, scene transitions and pause functionality. |
| `RotatePlanets.cs` | Planetary rotation. |
| `OrbitMaker.cs` | Planetary orbital movement logic. |
| `LaserMovement.cs` | Laser object movement and lifetime. |

**Note:** The complete Unity project, including scenes, prefabs, models, textures, audio, and other assets, is not included in this repository due to its large size and third-party asset distribution considerations.

The repository is intended as a source-code showcase rather than a complete, independently importable Unity project.

## Download and Play

A Windows build of the game is available in the repository's GitHub Releases section.

**[Download the Windows Build](GITHUB_RELEASE_LINK)**

Extract the downloaded ZIP file and run the game executable.

**Note:** The playable build is distributed separately from the source-code repository.

## Development Challenges and Problem Solving

Some of the main challenges encountered during development included:

- **Floating-Point Precision:** Addressed by implementing a floating-origin system that keeps the player near the world origin.
- **Three-Dimensional Enemy Navigation:** Adapted planar NavMesh navigation using an additional invisible navigation object and independent vertical movement.
- **Pause Menu Scene Management:** Used additive scene loading to preserve the active gameplay scene while displaying the pause menu.
- **Particle Effects During Pausing:** Used unscaled particle-system time to maintain background visual effects when gameplay was paused.

These challenges provided practical experience in debugging, adapting existing Unity systems, and implementing solutions for gameplay requirements.

## Acknowledgements

This project uses third-party assets and learning resources, including:

- [Planets of the Solar System 3D – Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/planets-of-the-solar-system-3d-90219)
- [Particle Pack – Unity Asset Store](https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325)
- [Electronic Videogame Music – Unity Asset Store](https://assetstore.unity.com/packages/audio/music/electronic/electronic-videogame-music-174009)
- [Unity VFX – Warp Drive / Hyperdrive Tutorial](https://www.youtube.com/watch?v=4hlCOUoc6aQ)
- [Waypoint Marker Tutorial](https://www.youtube.com/watch?v=oBkfujKPZw8)

Additional visual assets were sourced from Freepik and Icons8.

All third-party assets remain subject to their respective licences.

## Project Background

This project was developed as an individual university game-development project. It provided practical experience in Unity development, C# programming, 3D game mechanics, AI navigation, scene management, and solving technical challenges associated with large-scale game environments.

