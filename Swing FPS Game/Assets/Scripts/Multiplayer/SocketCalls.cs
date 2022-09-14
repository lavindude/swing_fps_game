public static class SocketCalls
{
    public static string PlayerInitData(string playerId)
    {
        return "{\"dataType\" : \"initializePlayer\", \"data\" : {\"playerId\" : " +
                 "\"" + playerId + "\", \"lobbyId\" : \"" + Constants.lobbyId + "\"}}";
    }

    public static string GetInitLobbyData()
    {
        return "{\"dataType\" : \"getLobbyData\", \"data\" : {\"playerId\" : \"" + Constants.playerId +
                 "\", \"lobbyId\" : \"" + Constants.lobbyId + "\"}}";
    }

    public static string SendPositionData(string playerDataJSON)
    {
        return "{\"dataType\" : \"playerPositionData\", \"data\" : {\"playerInfo\" : " + playerDataJSON + ", " +
                         "\"lobbyId\" : \"" + Constants.lobbyId + "\"}}";
    }
}
