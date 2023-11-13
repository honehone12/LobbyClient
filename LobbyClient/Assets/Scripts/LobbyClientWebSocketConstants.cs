namespace Lobby
{
    public static class LobbyClientWebSocketConstants
    {
        public const byte Request = 1;
        public const byte Notification = 2;

        public const byte Join = 0b0000_0001;
        public const byte Disconnect = 0b0000_0010;
        public const byte Chat = 0b0000_0100;

        public const string ChatMessage = "chat-message";
        public const string PlayerName = "player-name";
        public const string PlayerId = "player-id";
    }
}
