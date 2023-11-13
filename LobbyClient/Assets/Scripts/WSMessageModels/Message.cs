namespace Lobby
{
    [System.Serializable]
    public class Message
    {
        public string Key;
        public string Value;

        public Message(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
