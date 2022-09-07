# swing_fps_game

This will be a game that combines a spider-man swinging game and an fps. Most of the project contains unity related files but if you want to view any code, go to Assets/Scripts and there will be C# scripts in that directory and also any subdirectories.

Development notes:
Go to Assets/Scripts/Multiplayer/APIHelper.cs and adjust baseURL to either local or production according to what you're trying to fix or build.

Click <a href="https://github.com/lavindude/renovated_swing_backend">here</a> to see our first stable version of our multiplayer server. After some testing, we realized that http requests are pretty inefficient and slow so we are transitioning to a WebSocket based server (in progress).
