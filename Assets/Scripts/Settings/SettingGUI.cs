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
                LoadScene();
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
            LoadScene();
        }

        private void LoadScene()
        {
            Debug.Log($"{nameof(SettingGUI)} {nameof(LoadScene)}");
            SceneManager.LoadScene("Scenes/PlayScene");
        }
    }
}
