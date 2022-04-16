using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Controller;
using Settings;
using UnityEngine;

namespace Game
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField]
        public Player playerPrefab;

        public ControllerBase Controller { get; private set; }


        private void Start()
        {
            Setups.Add(SetupController);
        }

        private List<Func<Dictionary<string, string>, GameManager, CancellationToken, UniTask>> Setups = new();

        private async UniTask Setup()
        {
            var args = ArgumentParser.Args;
            var token = this.GetCancellationTokenOnDestroy();
            foreach (var task in Setups)
            {
                await task(args, this, token);
            }
        }


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
    }
}
