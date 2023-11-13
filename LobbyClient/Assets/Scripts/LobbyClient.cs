using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Lobby
{
    public class LobbyClient : MonoBehaviour
    {
        [SerializeField]
        UrlBuilder urlBuilder = new();
        
        LobbyClientWebSocketFunctions webSocketFunctions;
        LobbyClientHttpFunctions httpFunctions;

        void Awake()
        {
            httpFunctions = new(urlBuilder);
            webSocketFunctions = new(urlBuilder);
        }

        void Update()
        {
            webSocketFunctions.Update();
        }

        async void OnDestroy()
        {
            if (webSocketFunctions.IsActive)
            {
                try
                {
                    await webSocketFunctions.Close();
                }
                catch (System.Exception e)
                {
                    ErrorManager.Singleton.Exception(e);
                }
            }
        }

        public IEnumerator LobbyCreateRequest(string lobbyName, Box<LobbyCreateResponse> res)
        {
            return httpFunctions.LobbyCreateRequest(lobbyName, res);
        }

        public Coroutine StartLobbyCreateRequest(string lobbyName, UnityAction<LobbyCreateResponse> callback)
        {
            return StartCoroutine(httpFunctions.LobbyCreateRequestWithCallback(lobbyName, callback));
        }

        public IEnumerator LobbyDetailRequest(string lobbyId, Box<LobbyDetail> res)
        {
            return httpFunctions.LobbyDetailRequest(lobbyId, res);
        }

        public Coroutine StartLobbyDetailRequest(string lobbyId, UnityAction<LobbyDetail> callback)
        {
            return StartCoroutine(httpFunctions.LobbyDetailRequestWithCallback(lobbyId, callback));
        }

        public IEnumerator LobbyListRequest(Box<LobbyListResponse> res)
        {
            return httpFunctions.LobbyListRequest(res);
        }

        public Coroutine StartLobbyListRequest(UnityAction<LobbyListResponse> callback)
        {
            return StartCoroutine(httpFunctions.LobbyListRequestWithCallback(callback));
        }

        public IEnumerator LobbyJoinRequest(string lobbyId, string playerName, Box<LobbyJoinResponse> res)
        {
            return httpFunctions.LobbyJoinRequest(lobbyId, playerName, res);
        }

        public Coroutine StartLobbyJoinRequest(string lobbyId, string playerName, UnityAction<LobbyJoinResponse> callback)
        {
            return StartCoroutine(httpFunctions.LobbyJoinRequestWithCallback(lobbyId, playerName, callback));
        }

        public Task StartLobbyListen(string lobbyId, string playerId, UnityAction<Envelope> callback)
        {
            return webSocketFunctions.LobbyListen(lobbyId, playerId, callback);
        }

        public Task SendLobbyMessage(Envelope e)
        {
            return webSocketFunctions.SendMessage(e);
        }
    }
}
