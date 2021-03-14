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
        private int bounces = 0;
        private Rigidbody2D rigidbody;
        private Vector2 velocity;

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

            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            lifeTimer.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            velocity = rigidbody.velocity;
        }

        public void Reset()
        {
            lifeTimer.Reset();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Instantiate(Data.Boom, transform.position, Quaternion.identity);
            Debug.Log($"{name} collided with {collision.collider.name}.".Color(Color.grey));
            
            if (collision.gameObject.tag == Data.TargetTag /*&& bounces == Data.Bounces*/)
            {
                var health = collision.gameObject.GetComponent<Health>();
                health.Swallow(gameObject);

                health.Points -= Data.NutriotionValue;
            }
            else
            {
                if (bounces < Data.Bounces)
                {
                    var vector = Vector2.Reflect(velocity, collision.contacts[0].normal);
                    rigidbody.velocity = vector;

                    bounces++;
                }
                else
                {
                    Destroy(gameObject);
                }
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
