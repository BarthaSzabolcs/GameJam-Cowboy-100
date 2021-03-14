using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam.UI
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        [SerializeField] private TextMeshProUGUI calmedText;
        [SerializeField] private TextMeshProUGUI deadText;

        [SerializeField] private TextMeshProUGUI succesText;

        [SerializeField] private Button nextLevelButton;

        [SerializeField, TextArea] private string allCalmedMessage;
        [SerializeField, TextArea] private string allDeadMessage;
        [SerializeField, TextArea] private string deadPlayerMessage;
        [SerializeField, TextArea] private string halfSuccesMessage;

        private int nextSceneIndex;

        public void Show(bool playerDead, int enemies, int calmedEnemies, int deadEnemies, int nextLevelIndex)
        {
            canvas.gameObject.SetActive(true);

            Time.timeScale = 0;

            calmedText.text = $"Fed: {calmedEnemies}";
            deadText.text = $"Dead: {deadEnemies}";

            if (playerDead)
            {
                succesText.text = deadPlayerMessage;
            }
            else if (enemies == deadEnemies)
            {
                succesText.text = allDeadMessage;
            }
            else if(enemies == calmedEnemies)
            {
                succesText.text = allCalmedMessage;
            }
            else
            {
                succesText.text = halfSuccesMessage;
            }

            nextLevelButton.enabled = playerDead == false && enemies == calmedEnemies;
        }

        public void NextLevel()
        {
            if (nextSceneIndex != -1)
            {
                Time.timeScale = 1;
                
                SceneManager.LoadScene(nextSceneIndex);
            }
        }

        public void Restart()
        {
            Time.timeScale = 1;
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
