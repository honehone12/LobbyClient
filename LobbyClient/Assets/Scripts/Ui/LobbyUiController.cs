using UnityEngine;
using UnityEngine.Assertions;

namespace Lobby
{
    public class LobbyUiController : MonoBehaviour
    {
        public enum HttpRequestRoute
        {
            List,
            Detail,
            Create,
            Join
        }

        public enum WsMessageKind
        {
            Chat
        }

        public class HttpRequestParams
        {
            public HttpRequestRoute requestRoute;
            public string playerName;
            public string playerId;
            public string lobbyName;
            public string lobbyId;
        }

        public class WsMessageParams
        {
            public WsMessageKind messageKind;
            public string message;
        }

        [SerializeField]
        LobbyClient lobbyClient;
        [SerializeField]
        LobbyUiComponents components = new();

        HttpRequestParams reqParams = new();
        WsMessageParams wsParams = new();

        void Awake()
        {
            Assert.IsNotNull(lobbyClient);
            components.OnAwake();
            components.SwitchPanel(LobbyUiComponents.PanelState.BeforeJoin);
        }

        public void OnRequestRouteSelected(int newValue)
        {
            if (newValue < 0 || newValue > 3)
            {
                ErrorManager.Singleton.Error("unexpected value");
                return;
            }

            reqParams.requestRoute = (HttpRequestRoute)newValue;
        }

        public void OnMessageKindSelected(int newValue)
        {
            if (newValue < 0 || newValue > 0)
            {
                ErrorManager.Singleton.Error("unexpected value");
            }

            wsParams.messageKind = (WsMessageKind)newValue;
        }

        public void OnPlayerNameEdited(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return;
            }

            reqParams.playerName = newValue;
        }

        public void OnPlayerIdEdited(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return;
            }

            reqParams.playerId = newValue;
        }

        public void OnLobbyNameEdited(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return;
            }

            reqParams.lobbyName = newValue;
        }

        public void OnLobbyIdEdited(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return;
            }

            reqParams.lobbyId = newValue;
        }

        public void OnMessageFieldEdited(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return;
            }

            wsParams.message = newValue;
        }

        public void OnSendRequestButtonClicked()
        {
            switch (reqParams.requestRoute)
            {
                case HttpRequestRoute.List:
                    lobbyClient.StartLobbyListRequest((res) =>
                    {
                        components.SetOutputText($"LobbyCount: {res.LobbyCount}");
                        components.AddOutputText("[");
                        for (int i = 0, length = res.List.Length; i < length; i++)
                        {
                            var l = res.List[i];
                            components.AddOutputText($"Id: {l.Id}\nName: {l.Name}\nPlayerCount: {l.PlayerCount}\nActiveCount: {l.ActiveCount}");
                        }
                        components.AddOutputText("]");
                    });
                    break;
                case HttpRequestRoute.Detail:
                    lobbyClient.StartLobbyDetailRequest(reqParams.lobbyId, (res) =>
                    {
                        components.SetOutputText($"Id: {res.Id}\nName: {res.Name}\nPlayerCount: {res.PlayerCount}\nActiveCount: {res.ActiveCount}");
                        components.AddOutputText("[");
                        for (int i = 0, length = res.PlayerList.Length; i < length; i++)
                        {
                            var p = res.PlayerList[i];
                            components.AddOutputText($"Id: {p.Id}\nName: {p.Name}\nActive: {p.Active}");
                        }
                        components.AddOutputText("]");
                    });
                    break;
                case HttpRequestRoute.Create:
                    lobbyClient.StartLobbyCreateRequest(reqParams.lobbyName, (res) =>
                    {
                        components.SetOutputText($"LobbyId: {res.LobbyId}");
                    });
                    break;
                case HttpRequestRoute.Join:
                    lobbyClient.StartLobbyJoinRequest(reqParams.lobbyId, reqParams.playerName, async (res) =>
                    {
                        components.SwitchPanel(LobbyUiComponents.PanelState.AfterJoin);
                        reqParams.playerId = res.PlayerId;

                        await lobbyClient.StartLobbyListen(reqParams.lobbyId, reqParams.playerId, (e) =>
                        {
                            components.SetOutputText("");
                            if (e.GetFlag(LobbyClientWebSocketConstants.Join))
                            {
                                components.AddOutputText("Join flag");
                            }
                            if (e.GetFlag(LobbyClientWebSocketConstants.Chat))
                            {
                                components.AddOutputText("Chat flag");
                            }

                            components.AddOutputText("[");
                            for (int i = 0, length = e.Messages.Length; i < length; i++)
                            {
                                var msg = e.Messages[i];
                                components.AddOutputText("{");
                                components.AddOutputText($"{msg.Key}: {msg.Value}");
                                components.AddOutputText("}");
                            }
                            components.AddOutputText("]");
                        });
                    });
                    break;
                default:
                    ErrorManager.Singleton.Error("unexpected request route");
                    return;
            }
        }

        public async void OnSendMessageButtonClicked()
        {
            switch (wsParams.messageKind)
            {
                case WsMessageKind.Chat:
                    await lobbyClient.SendLobbyMessage(Envelope.ChatMessage(wsParams.message));
                    break;
                default:
                    throw new System.Exception("unexpected value");
            }
        }
    }
}
