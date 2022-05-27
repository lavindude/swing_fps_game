using UnityEngine.Networking;

public static class Constants
{
    /* We want these values to be dynamic in the future through lobbymaking.
       For now, they are hard coded. */
    public static int playerId = 4;
    public static int lobbyId = 1;
    public static int[] otherPlayerIds = new int[] { 1, 2, 3 };
}