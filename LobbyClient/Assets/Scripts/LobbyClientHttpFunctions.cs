using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace Lobby
{
    public class LobbyClientHttpFunctions
    {
        readonly UrlBuilder urlBuilder;

        public LobbyClientHttpFunctions(UrlBuilder urlBuilder)
        {
            this.urlBuilder = urlBuilder;
        }

        public IEnumerator LobbyCreateRequestWithCallback(string lobbyName, UnityAction<LobbyCreateResponse> callback)
        {
            var box = new Box<LobbyCreateResponse>();
            yield return LobbyCreateRequest(lobbyName, box);
            callback?.Invoke(box.value);
        }

        public IEnumerator LobbyCreateRequest(string lobbyName, Box<LobbyCreateResponse> res)
        {
            Assert.IsNotNull(res);
            var form = new WWWForm();
            form.AddField("lobby-name", lobbyName);
            var req = UnityWebRequest.Post(urlBuilder.MakeHttpUrl("/lobby/create"), form);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                ErrorManager.Singleton.Error(req.error);
                yield break;
            }

            var body = req.downloadHandler.text;
            Debug.Log(body);
            res.value = JsonUtility.FromJson<LobbyCreateResponse>(body);
        }

        public IEnumerator LobbyDetailRequestWithCallback(string lobbyId, UnityAction<LobbyDetail> callback)
        {
            var box = new Box<LobbyDetail>();
            yield return LobbyDetailRequest(lobbyId, box);
            callback?.Invoke(box.value);
        }

        public IEnumerator LobbyDetailRequest(string lobbyId, Box<LobbyDetail> res)
        {
            Assert.IsNotNull(res);
            var form = new WWWForm();
            form.AddField("lobby-id", lobbyId);
            var req = UnityWebRequest.Post(urlBuilder.MakeHttpUrl("/lobby/detail"), form);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                ErrorManager.Singleton.Error(req.error);
                yield break;
            }

            var body = req.downloadHandler.text;
            Debug.Log(body);
            res.value = JsonUtility.FromJson<LobbyDetail>(body);
        }

        public IEnumerator LobbyListRequestWithCallback(UnityAction<LobbyListResponse> callback)
        {
            var box = new Box<LobbyListResponse>();
            yield return LobbyListRequest(box);
            callback?.Invoke(box.value);
        }

        public IEnumerator LobbyListRequest(Box<LobbyListResponse> res)
        {
            Assert.IsNotNull(res);
            var req = UnityWebRequest.Get(urlBuilder.MakeHttpUrl("/lobby/list"));
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                ErrorManager.Singleton.Error(req.error);
                yield break;
            }

            var body = req.downloadHandler.text;
            Debug.Log(body);
            res.value = JsonUtility.FromJson<LobbyListResponse>(body);
        }

        public IEnumerator LobbyJoinRequestWithCallback(string lobbyId, string playerName, UnityAction<LobbyJoinResponse> callback)
        {
            var box = new Box<LobbyJoinResponse>();
            yield return LobbyJoinRequest(lobbyId, playerName, box);
            callback?.Invoke(box.value);
        }

        public IEnumerator LobbyJoinRequest(string lobbyId, string playerName, Box<LobbyJoinResponse> res)
        {
            Assert.IsNotNull(res);
            var form = new WWWForm();
            form.AddField("lobby-id", lobbyId);
            form.AddField("player-name", playerName);
            var req = UnityWebRequest.Post(urlBuilder.MakeHttpUrl("/lobby/join"), form);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                ErrorManager.Singleton.Error(req.error);
                yield break;
            }

            var body = req.downloadHandler.text;
            Debug.Log(body);
            res.value = JsonUtility.FromJson<LobbyJoinResponse>(body);
        }
    }
}
