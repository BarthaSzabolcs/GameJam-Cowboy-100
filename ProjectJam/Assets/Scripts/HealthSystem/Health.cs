using System;
using System.Collections.Generic;

using UnityEngine;

using BarthaSzabolcs.RichTextHelper;
using GameJam.FoodGun;

namespace GameJam.HealthSystem
{
    public class Health : MonoBehaviour
    {
        #region Datamembers

        #region Events

        public event Action OnCalm;
        public event Action OnDeath;
        public event Action<int> OnHealthChange;

        #endregion
        #region Editor Settings

        [field: SerializeField]
        public HealthData Data { get; private set; }

        [SerializeField] private SpriteFlash flash;
        [SerializeField] private ParticleSystem boom;

        #endregion
        #region Public Properties

        public int Points
        {
            set
            {
                OnHealthChange?.Invoke(value);

                if (value <= 0)
                {
                    Explode();
                }
                else if (value == 1)
                {
                    _points = value;

                    CalmDown();
                }
                else
                {
                    Debug.Log($"{name}'s health changed from {_points} to {value}.".Color(Color.yellow));

                    flash.StartFlash(Color.red);
                    _points = value;
                }
            }
            get
            {
                return _points;
            }
        }

        public bool Calmed => Points == 1;

        #endregion
        #region Backing Fields

        private int _points;

        #endregion
        #region Private Fields

        private List<GameObject> swallowedObjects = new List<GameObject>();

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            _points = Data.MaxHealth;
        }

        #endregion

        private void CalmDown()
        {
            OnCalm?.Invoke();
            flash.StartFlash(Color.green);

            Debug.Log($"{name}'s health reached shrinked.".Color(Color.cyan));
        }

        private void Explode()
        {
            Debug.Log($"{name}'s health reached shrinked.".Color(Color.magenta));
            
            foreach (var swallowedObject in swallowedObjects)
            {
                swallowedObject.SetActive(true);
                swallowedObject.transform.position = transform.position;
                var rBody = swallowedObject.GetComponent<Rigidbody2D>();

                var vector = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                rBody.velocity = vector.normalized * 10f;
            }

            Destroy();
        }

        public void Destroy()
        {
            OnDeath?.Invoke();
            Instantiate(boom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void Swallow(GameObject swallowedObject)
        {
            swallowedObjects.Add(swallowedObject);
            swallowedObject.SetActive(false);
            swallowedObject.GetComponent<Food>().Reset();
        }

        #endregion
    }
}
