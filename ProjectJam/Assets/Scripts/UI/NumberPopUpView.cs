using TMPro;

using UnityEngine;

using BarthaSzabolcs.CommonUtility;

namespace GameJam.UI
{
    public class NumberPopUpView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI number;
        [SerializeField] private float lifeTime;

        private Timer timer = new Timer();

        public void Show(int value, Color color)
        {
            number.color = color;
            number.text = value.ToString();
        }

        private void Start()
        {
            timer.Interval = lifeTime;
            timer.Init();

            timer.OnTimeElapsed += SelfDestroy;
        }

        private void Update()
        {
            timer.Tick(Time.deltaTime);
        }

        private void SelfDestroy()
        {
            Destroy(gameObject);

            timer.OnTimeElapsed -= SelfDestroy;
        }
    }
}
