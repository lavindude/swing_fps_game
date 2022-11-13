# Swing FPS Game

This is a game that combines a spider-man swinging game and an fps.

![image](https://user-images.githubusercontent.com/60680607/201507020-effcf4b3-713d-4ab1-b787-ab7847f7c258.png)

Download our game here: https://lavindude.itch.io/swing-fps-game <br>

Download instructions:
1. Click on the itch.io link above
2. Download the zip
3. Extract the zip
4. Click on the application Swing FPS Game
5. Once the game opens, pick any name, JOIN LOBBY 1, and make sure your playerId is different from your teammates.
6. Enjoy!

Most of the project contains unity related files but if you want to view any code, go to Assets/Scripts and there will be C# scripts in that directory and also any subdirectories.

Click <a href="https://github.com/lavindude/renovated_swing_backend">here</a> to see our first stable version of our multiplayer server. After some testing, we realized that a REST API is pretty inefficient and slow for a multiplayer first person shooter game when there are 4 players or more so we are transitioning to a more efficient WebSocket based server.

Development notes:
Go to Assets/Scripts/Multiplayer/APIHelper.cs and adjust baseURL to either local or production according to what you're trying to fix or build.
