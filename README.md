# Ex7-3d-terrain-ai
This project is part of a weekly assignment designed to develop a 3D world in Unity. As part of this assignment, I had to choose between several tasks. I chose to improve the Enemy AI and expand the scene with new structures and features. 

## Part 1: Advanced Enemy AI
The enemy from class was initially programmed with simple, random navigation. This part improves the AI by introducing three possible behaviors:
* Escape Mode (Cowardly Enemy):
The enemy selects the target point farthest from the player.
* Guard Mode (Brave Enemy):
The enemy selects the target point closest to the player.
* Attack the Engine:
The enemy navigates toward the engine in the center of the building.

These behaviors are implemented using a state machine, where the enemy transitions between states based on certain conditions (e.g., distance to the player, probabilities).

### Instructions
* Use the arrow keys to move the player around the map.
* Use the mouse to rotate and change the player's direction.
* Observe how the enemy reacts to your position based on its current state.

### itch.io: https://matanyocheved.itch.io/ex7-3d-terrain-ai-part1


## Part 2: Scene Expansion
A new sci-fi-themed room was added to the game environment, connected to the main structure by a corridor and steps. The room was built and aligned using ProGrids and ProBuilder, with custom objects like a portal added for detail. Lighting was also included to improve visibility and atmosphere.

### Instructions
* Use the arrow keys to move the player around the map.
* Use the mouse to rotate and change the player's direction.

### itch.io: https://matanyocheved.itch.io/ex7-3d-terrain-part2

