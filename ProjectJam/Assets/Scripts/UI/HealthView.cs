using UnityEngine;
using UnityEngine.UI;

using TMPro;

using GameJam.HealthSystem;

namespace GameJam.UI
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Health health;

        [SerializeField] private Image hungerBar;
        [SerializeField] private TextMeshProUGUI hungerText;
        [SerializeField] private GameObject popUpPrefab;

        [SerializeField] private Color hungryColor;
        [SerializeField] private Color fedColor;

        private int currentHunger;

        private void Start()
        {
            currentHunger = health.Data.MaxHealth;

            Refresh(currentHunger);

            health.OnHealthChange += Refresh;
        }

        private void Refresh(int newHunger)
        {
            var color = newHunger != 1 ? hungryColor : fedColor;
            hungerBar.fillAmount = (float)newHunger / health.Data.MaxHealth;
            hungerBar.color = color;

            hungerText.text = newHunger.ToString();
            hungerText.color = color;

            if (currentHunger != newHunger)
            {
                var popUpInstance = Instantiate(popUpPrefab, transform.position, Quaternion.identity);
                var popUpView = popUpInstance.GetComponent<NumberPopUpView>();

                popUpView.Show(newHunger - currentHunger, color);
            }

            currentHunger = newHunger;
        }
    }
}
