using UnityEngine;

using BarthaSzabolcs.RichTextHelper;

using GameJam.HealthSystem;

namespace GameJam.FoodGun
{
    public class Food : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [SerializeField] private FoodData data;

        #endregion

        #endregion


        #region Methods
        
        #region Unity Callbacks

        private void Start()
        {
            var rBody = GetComponent<Rigidbody>();
            rBody.velocity = transform.forward * data.Speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"{name} collided with {collision.collider.name}.".Color(Color.grey));
            
            if (collision.gameObject.tag == data.TargetTag)
            {
                var health = collision.gameObject.GetComponent<Health>();
                health.Points -= data.NutriotionValue;
            }

            Destroy(gameObject);
        }

        #endregion

        #endregion

    }
}
