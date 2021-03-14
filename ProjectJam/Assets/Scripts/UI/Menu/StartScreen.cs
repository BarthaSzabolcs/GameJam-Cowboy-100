using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam.UI
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField, TextArea] private string message;


        public void Show()
        {
            canvas.gameObject.SetActive(true);

            Time.timeScale = 0;

            messageText.text = message;
        }


        private void Update()
        {
            if (Input.GetKeyDown("space") && canvas.gameObject.activeSelf)
            {
                Time.timeScale = 1;
                canvas.gameObject.SetActive(false);
            }
        }
    }
}
