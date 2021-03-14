using UnityEngine;

using BarthaSzabolcs.RichTextHelper;
using BarthaSzabolcs.CommonUtility;

using GameJam.HealthSystem;

namespace GameJam.FoodGun
{
    public class Food : MonoBehaviour
    {
        #region Datamembers

        #region Public Properties

        public FoodData Data 
        { 
            get
            {
                return _data;
            }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    InitData(_data);
                }
            }
        }

        #endregion
        #region Backing Fields

        private FoodData _data;

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
            rBody.velocity = transform.right * Data.Speed;

            lifeTimer.OnTimeElapsed += () => Destroy(gameObject);
            lifeTimer.Interval = Data.LifeTime;
            lifeTimer.Init();
        }

        private void Update()
        {
            lifeTimer.Tick(Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"{name} collided with {collision.collider.name}.".Color(Color.grey));
            
            if (collision.gameObject.tag == Data.TargetTag)
            {
                var health = collision.gameObject.GetComponent<Health>();
                health.Swallow(gameObject);

                health.Points -= Data.NutriotionValue;
            }
        }

        private void InitData(FoodData data)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = data.Sprite;

            var circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.radius = data.Radius;
        }

        #endregion

        #endregion

    }
}
