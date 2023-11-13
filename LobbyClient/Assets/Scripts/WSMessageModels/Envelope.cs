namespace Lobby
{
    [System.Serializable]
    public class Envelope
    {
        public static Envelope ChatMessage(string message)
        {
            return new Envelope(
                LobbyClientWebSocketConstants.Request,
                LobbyClientWebSocketConstants.Chat,
                new Message[]
                {
                    new Message(LobbyClientWebSocketConstants.ChatMessage, message)
                }
            );
        }

        public byte Direction;
        public byte Flag;
        public Message[] Messages;

        public Envelope(byte direction, byte flag, Message[] messages)
        {
            Direction = direction;
            Flag = flag;
            Messages = messages;
        }

        public bool GetFlag(byte bit)
        {
            return (Flag & bit) == bit;
        }

        public bool GetMessage(string key, out string value)
        {
            var msg = System.Array.Find(Messages, (m) => m.Key == key);
            if (msg == null) 
            {
                value = string.Empty;
                return false;
            }

            value = msg.Value;
            return true;
        }
    }
}
