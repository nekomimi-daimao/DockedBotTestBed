using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public sealed class SettingGUI : MonoBehaviour
    {
        [SerializeField]
        private Button loadGame;

        private void Start()
        {
            Init();
        }

        [Conditional("UNITY_SERVER")]
        private void Init()
        {
        }
    }
}
