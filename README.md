
# Integrating GameShift with Unity

This document provides guidelines on integrating GameShift, into your Unity game to handle transactions and game state securely and efficiently.

## Recommended Game Architecture

When integrating GameShift into your Unity game, we recommend the following architecture:

![image](https://github.com/harshasomisetty/cube_crusher/assets/25572511/f6640649-be46-41ad-9759-032c3e3ca5c8)


- **Game Client:** A Unity application that the player interacts with. It is responsible for presenting the game state and handling user inputs.
- **Server:** Your custom backend server acts as an intermediary between the Game Client and GameShift API. It is crucial for several reasons:
  - *Security:* The server ensures that sensitive GameShift API calls are not exposed to the client, reducing the risk of unauthorized access and manipulation.
  - *User Profiling:* Stores persistent user data that lives with the game, such as profiles, progress, and statistics.
  - *Custom Game Logic:* Allows the implementation of custom game logic that is not handled by the Game Client or GameShift API. Utilizing a server will make this data processing faster than in Unity.
 - **GameShift:** Gameshift will contain the canonical on-chain data for your game

The server plays a major role in this architecture by processing game logic, securely interacting with the GameShift API, and updating the game state based on the responses.

## Workflow

The interaction between the Game Client, Server, and GameShift API typically follows these steps. We've included sample code to demonstrate a potential integration—the github repo is available [here](https://github.com/harshasomisetty/cube_crusher/tree/main), we will follow sample code to show how a player might be awarded an nft after clearing a level

1. **Action Sending:** The Game Client sends player actions to the Server. [code](https://github.com/harshasomisetty/cube_crusher/blob/438d3047d4b86a3a4c5ec958b642a0490cbef2c4/3dgame/Assets/Scripts/Data/NetworkService.cs#L49)

2. **Data Processing:** The Server processes these actions and sends relevant requests to the GameShift API, and receives an appropriate response. [code](https://github.com/harshasomisetty/cube_crusher/blob/438d3047d4b86a3a4c5ec958b642a0490cbef2c4/server/src/routes/user.ts#L156)

3. **State Update:** The Server updates the game state and sends it back to the Game Client. [code](https://github.com/harshasomisetty/cube_crusher/blob/438d3047d4b86a3a4c5ec958b642a0490cbef2c4/3dgame/Assets/Scripts/Menus/EndMenu.cs#L65)
