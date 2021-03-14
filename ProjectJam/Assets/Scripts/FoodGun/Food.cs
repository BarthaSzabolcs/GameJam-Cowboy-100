using UnityEngine;

using BarthaSzabolcs.RichTextHelper;
using BarthaSzabolcs.CommonUtility;

using GameJam.HealthSystem;

namespace GameJam.FoodGun
{
    public class Food : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [SerializeField] private FoodData data;

        #endregion
        #region Private Fields

        private Timer lifeTimer = new Timer();
        
        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            var rBody = GetComponent<Rigidbody2D>();
            rBody.velocity = transform.right * data.Speed;

            lifeTimer.OnTimeElapsed += () => Destroy(gameObject);
            lifeTimer.Interval = data.LifeTime;
            lifeTimer.Init();
        }

        private void Update()
        {
            lifeTimer.Tick(Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"{name} collided with {collision.collider.name}.".Color(Color.grey));
            
            if (collision.gameObject.tag == data.TargetTag)
            {
                var health = collision.gameObject.GetComponent<Health>();
                health.Swallow(gameObject);

                health.Points -= data.NutriotionValue;
            }
        }

        #endregion

        #endregion

    }
}
