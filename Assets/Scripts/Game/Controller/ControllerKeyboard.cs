using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Controller
{
    public class ControllerKeyboard : ControllerBase
    {
        public override void Start()
        {
            base.Start();
            CheckInput(CancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid CheckInput(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield();

                var stick = Vector2.zero;

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    stick += Vector2.right;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    stick += Vector2.left;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    stick += Vector2.up;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    stick += Vector2.down;
                }

                StickForce.Value = stick.normalized;
                JumpPushed.Value = Input.GetKey(KeyCode.Z);
            }
        }
    }
}
