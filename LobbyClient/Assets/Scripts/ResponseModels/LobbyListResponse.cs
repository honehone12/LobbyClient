namespace Lobby
{
    [System.Serializable]
    public class LobbyListResponse
    {
        public uint LobbyCount;
        public LobbySummary[] List;
    }
}
