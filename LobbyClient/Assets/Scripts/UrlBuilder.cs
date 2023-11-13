using UnityEngine;

namespace Lobby
{
    [System.Serializable]
    public class UrlBuilder
    {
        [SerializeField]
        string address;
        [SerializeField]
        string port;

        public string MakeHttpUrl(string route)
        {
            return "http://" + address + ":" + port + route;
        }

        public string MakeWebSocketUrl(string route)
        {
            return "ws://" + address + ":" + port + route;
        }
    }
}
