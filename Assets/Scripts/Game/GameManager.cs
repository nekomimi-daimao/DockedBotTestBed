using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Controller;
using Game.Setup;
using Settings;
using UnityEngine;

namespace Game
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField]
        public Player playerPrefab;

        public Player Player { get; set; }

        public ControllerBase Controller { get; private set; }

        public string Name { get; private set; } = "NoName";

        private async UniTaskVoid Start()
        {
            Func<Dictionary<string, string>, GameManager, CancellationToken, UniTask>[] setupDefault =
            {
                SetupController,
                SetupName,
            };
            SetupList.AddRange(setupDefault);
            SetupList.Add(SetupHolder.SDKSetup());

            var token = this.GetCancellationTokenOnDestroy();

            await StartSetup(token);
            await UniTask.WaitUntil(() => Player != null, cancellationToken: token);
            Player.SetController(Controller, token);
        }

        // private UniTask SetupXXX(Dictionary<string, string> args, GameManager gameManager, CancellationToken token)
        private List<Func<Dictionary<string, string>, GameManager, CancellationToken, UniTask>> SetupList = new();

        private async UniTask StartSetup(CancellationToken token)
        {
            var args = ArgumentParser.Args;
            foreach (var task in SetupList)
            {
                await task(args, this, token);
            }
        }

        #region GameManagerSetup

        private UniTask SetupController(Dictionary<string, string> args, GameManager gameManager, CancellationToken token)
        {
            var isBot = false;
            if (args.TryGetValue("isBot", out var s) && bool.TryParse(s, out var result))
            {
                isBot = result;
            }
            gameManager.Controller = isBot ? new ControllerBot() : new ControllerKeyboard();

            return UniTask.CompletedTask;
        }

        private UniTask SetupName(Dictionary<string, string> args, GameManager gameManager, CancellationToken token)
        {
            if (args.TryGetValue("name", out var s))
            {
                gameManager.Name = s;
            }
            return UniTask.CompletedTask;
        }

        #endregion
    }
}
