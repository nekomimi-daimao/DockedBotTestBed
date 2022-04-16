using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Settings
{
    public sealed class SettingGUI : MonoBehaviour
    {
        [SerializeField]
        private Button loadGame;

        [SerializeField]
        private RectTransform canvasRoot;

        private void Start()
        {
            if (ArgumentParser.IsBot())
            {
                SceneManager.LoadScene("Scenes/PlayScene");
                return;
            }
            loadGame.onClick.AddListener(Load);
        }

        private void Load()
        {
            foreach (var kv in canvasRoot.GetComponentsInChildren<KeyValueInputs>())
            {
                var k = kv.Key.text;
                var v = kv.Value.text;
                if (string.IsNullOrEmpty(k))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(v))
                {
                    v = null;
                }
                ArgumentParser.Args[k] = v;
            }

            SceneManager.LoadScene("Scenes/PlayScene");
        }
    }
}
