#if PHOTON_UNITY_NETWORKING

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Game.Setup.Photon
{
    public static class SetupPhoton
    {
        public static async UniTask Setup(Dictionary<string, string> args, GameManager gameManager, CancellationToken token)
        {
            BackUp(token);

            args.TryGetValue("realtime", out var realtime);
            args.TryGetValue("voice", out var voice);
            args.TryGetValue("matching", out var roomname);

            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = realtime;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = voice;

            PhotonNetwork.PrefabPool = new GamePool(gameManager);

            PhotonNetwork.ConnectUsingSettings();

            await UniTask.WaitUntil(() => PhotonNetwork.IsConnectedAndReady, cancellationToken: token);

            PhotonNetwork.JoinOrCreateRoom(roomname, new RoomOptions(), TypedLobby.Default);

            await UniTask.WaitUntil(() => PhotonNetwork.InRoom, cancellationToken: token);

            var player = PhotonNetwork.Instantiate(
                null,
                Vector3.up,
                Quaternion.identity,
                0,
                new object[] { gameManager.Name }
            );

            gameManager.Player = player.GetComponent<Player>();
        }

        private class GamePool : IPunPrefabPool
        {
            private readonly Player _playerPrefab;

            public GamePool(GameManager gameManager)
            {
                _playerPrefab = Object.Instantiate(gameManager.playerPrefab);
                var go = _playerPrefab.gameObject;
                go.SetActive(false);
                var photonView = go.AddComponent<PhotonView>();
                photonView.Synchronization = ViewSynchronization.UnreliableOnChange;

                var photonTransform = go.AddComponent<PhotonTransformView>();
                photonTransform.m_SynchronizePosition = true;
                photonTransform.m_SynchronizeRotation = true;
                photonTransform.m_SynchronizeScale = false;

                var photonRigid = go.AddComponent<PhotonRigidbodyView>();
                photonRigid.m_TeleportEnabled = true;
                photonRigid.m_SynchronizeVelocity = true;
                photonRigid.m_SynchronizeAngularVelocity = true;

                photonView.ObservedComponents ??= new List<Component>();
                photonView.ObservedComponents.Clear();
                photonView.ObservedComponents.Add(photonTransform);
                photonView.ObservedComponents.Add(photonRigid);

                go.AddComponent<PhotonCallbackHandler>();
            }

            public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
            {
                var instance = Object.Instantiate(_playerPrefab, position, rotation);
                return instance.gameObject;
            }

            public void Destroy(GameObject gameObject)
            {
                Object.Destroy(gameObject);
            }
        }

        [Conditional("UNITY_EDITOR")]
        private static async void BackUp(CancellationToken token)
        {
            var realtime = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            var voice = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;

            await token.WaitUntilCanceled();

            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = realtime;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = voice;
        }
    }
}

#endif
