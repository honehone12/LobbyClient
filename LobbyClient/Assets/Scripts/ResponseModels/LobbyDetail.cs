namespace Lobby
{
    [System.Serializable]
    public class LobbyDetail
    {
        public string Id;
        public string Name;
        public uint PlayerCount;
        public uint ActiveCount;
        public PlayerInfo[] PlayerList;
    }
}
