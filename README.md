# Swing FPS Game

Download our game here: https://lavindude.itch.io/swing-fps-game <br>
-Multiplayer is not enabled right now because the servers are off.

This is a game that combines a spider-man swinging game and an fps. Most of the project contains unity related files but if you want to view any code, go to Assets/Scripts and there will be C# scripts in that directory and also any subdirectories.

Click <a href="https://github.com/lavindude/renovated_swing_backend">here</a> to see our first stable version of our multiplayer server. After some testing, we realized that a REST API is pretty inefficient and slow for a multiplayer first person shooter game when there are 4 players or more so we are transitioning to a more efficient WebSocket based server.

Development notes:
Go to Assets/Scripts/Multiplayer/APIHelper.cs and adjust baseURL to either local or production according to what you're trying to fix or build.
