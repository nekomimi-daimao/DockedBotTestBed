using System.Threading;
using UniRx;

namespace Game.Controller
{
    public abstract class ControllerBase
    {
        public Vector2ReactiveProperty StickForce { get; } = new();
        public BoolReactiveProperty JumpPushed { get; } = new();

        protected CancellationTokenSource CancellationTokenSource;

        public virtual void Start()
        {
            if (CancellationTokenSource is { IsCancellationRequested: false })
            {
                CancellationTokenSource.Cancel();
                CancellationTokenSource = null;
            }
            CancellationTokenSource = new CancellationTokenSource();
        }

        public virtual void Stop()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = null;
        }
    }
}
