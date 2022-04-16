using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public sealed class KeyValueInputs : MonoBehaviour
    {
        [SerializeField]
        public InputField Key;

        [SerializeField]
        public InputField Value;
    }
}
