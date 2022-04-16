using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Setup.Photon;
using Game;

namespace Game.Setup
{
    public static class SetupHolder
    {
        public static Func<Dictionary<string, string>, GameManager, CancellationToken, UniTask> SDKSetup()
        {
#if PHOTON_UNITY_NETWORKING
            return SetupPhoton.Setup;
#endif

#pragma warning disable CS0162
            return (_, _, _) => UniTask.CompletedTask;
#pragma warning restore CS0162
        }
    }
}
