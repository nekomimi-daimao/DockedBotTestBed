using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Controller;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class Player : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigid;

        [SerializeField]
        private Renderer rend;

        private Material _material;

        [Header("name")]
        [SerializeField]
        private Canvas canvasName;

        [SerializeField]
        private Text textName;

        #region Initialize

        public void Init(string userName, ControllerBase controller, CancellationToken token)
        {
            InitMaterial(userName);
            InitCanvas(userName);
            InitController(controller, token).Forget();
        }

        private void InitMaterial(string userName)
        {
            var hash = userName.GetHashCode();
            var digit6 = GetPointDigit(hash, 6);
            var digit5 = GetPointDigit(hash, 5);
            var digit4 = GetPointDigit(hash, 4);
            var digit3 = GetPointDigit(hash, 3);
            var digit2 = GetPointDigit(hash, 2);
            var digit1 = GetPointDigit(hash, 1);

            _material = rend.material;
            _material.color = Color.HSVToRGB(
                digit6 * 0.1f + digit5 * 0.01f,
                digit4 * 0.1f + digit3 * 0.01f,
                digit2 * 0.1f + digit1 * 0.01f);

            int GetPointDigit(int num, int digit)
            {
                return (int)(num / Mathf.Pow(10, digit - 1)) % 10;
            }

            gameObject.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    if (_material != null)
                    {
                        Destroy(_material);
                    }
                });
        }

        private void InitCanvas(string userName)
        {
            textName.text = userName;
            var cam = Camera.main;
            if (cam == null)
            {
                return;
            }
            canvasName.worldCamera = cam;
            var cameraTs = cam.transform;
            var canvasTs = canvasName.gameObject.GetComponent<RectTransform>();
            gameObject.transform
                .ObserveEveryValueChanged(ts => ts.position, FrameCountType.FixedUpdate)
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => canvasTs.LookAt(cameraTs));
        }

        private async UniTask InitController(ControllerBase controller, CancellationToken token)
        {
            controller.Start();
            await UniTask.Yield();
            MoveLoop(controller, token).Forget();
            JumpLoop(controller, token).Forget();
        }

        #endregion

        #region Move

        private const float MoveForce = 10f;
        private const float ForceRatio = 0.3f;

        private async UniTaskVoid MoveLoop(ControllerBase controller, CancellationToken token)
        {
            var stickForce = controller.StickForce;

            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForFixedUpdate(token);

                var input = stickForce.Value;
                if (input == Vector2.zero)
                {
                    continue;
                }

                var v = new Vector3(input.x, 0f, input.y);
                if (v.sqrMagnitude > 1f)
                {
                    v = v.normalized;
                }
                rigid.velocity = Vector3.Lerp(rigid.velocity, v * MoveForce, ForceRatio);
            }
        }

        #endregion

        #region Jump

        private const float JumpForce = 20f;
        private const float JumpInterval = 0.5f;

        private async UniTaskVoid JumpLoop(ControllerBase controller, CancellationToken token)
        {
            var jumpPushed = controller.JumpPushed;

            while (!token.IsCancellationRequested)
            {
                var jump = await jumpPushed;

                if (token.IsCancellationRequested)
                {
                    break;
                }
                if (!jump)
                {
                    continue;
                }
                var velocity = rigid.velocity;
                velocity.y = 0f;
                rigid.velocity = velocity;
                rigid.AddForce(Vector3.up * JumpForce);

                await UniTask.Delay(TimeSpan.FromSeconds(JumpInterval), cancellationToken: token);
            }
        }

        #endregion
    }
}
