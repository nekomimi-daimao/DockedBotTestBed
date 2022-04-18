using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Controller
{
    public class ControllerBot : ControllerBase
    {
        private const int IntervalSecond = 1;

        public override void Start()
        {
            base.Start();
            RandomValue(CancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid RandomValue(CancellationToken token)
        {
            UnityEngine.Random.InitState(new System.Random().Next());
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(IntervalSecond), cancellationToken: token);
                StickForce.Value = UnityEngine.Random.insideUnitCircle;
                JumpPushed.Value = !JumpPushed.Value;
            }
        }
    }
}
