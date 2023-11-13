using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using NativeWebSocket;

namespace Lobby
{
    public class LobbyClientWebSocketFunctions
    {
        readonly UrlBuilder urlBuilder;
        WebSocket webSocket;

        public LobbyClientWebSocketFunctions(UrlBuilder urlBuilder)
        {
            this.urlBuilder = urlBuilder;
        }

        public bool IsActive => webSocket != null;

        public void Update()
        {
            webSocket?.DispatchMessageQueue();
        }

        public Task Close()
        {
            return webSocket?.Close();
        }

        public Task LobbyListen(string lobbyId, string playerId, UnityAction<Envelope> callback)
        {
            var route = $"/lobby/listen/{lobbyId}?player={playerId}";
            webSocket = new(urlBuilder.MakeWebSocketUrl(route));
            webSocket.OnOpen += () => Debug.Log("websocket successfully opened");
            webSocket.OnClose += (code) => Debug.Log($"websocket closed with code {code}");
            webSocket.OnError += (e) => ErrorManager.Singleton.Error(e);
            webSocket.OnMessage += (b) => OnListeningMessage(b, callback);
            return webSocket.Connect();
        }

        void OnListeningMessage(byte[] raw, UnityAction<Envelope> callback)
        {
            var body = Encoding.UTF8.GetString(raw);
            Debug.Log(body);
            var envelope = JsonUtility.FromJson<Envelope>(body);
            if (envelope.Direction != LobbyClientWebSocketConstants.Notification || 
                envelope.Flag == 0 ||
                envelope.Messages == null)
            {
                return;
            }

            callback?.Invoke(envelope);
        }

        public Task SendMessage(Envelope e)
        {
            var text = JsonUtility.ToJson(e);
            return webSocket.SendText(text);
        }
    }
}
