using System;
using System.Collections.Generic;

using UnityEngine;

using BarthaSzabolcs.RichTextHelper;

namespace GameJam.HealthSystem
{
    public class Health : MonoBehaviour
    {
        #region Datamembers

        #region Events

        public event Action OnCalm;

        #endregion
        #region Editor Settings

        [SerializeField] private HealthData data;

        #endregion
        #region Public Properties

        public int Points
        {
            set
            {
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
                    _points = value;
                }
            }
            get
            {
                return _points;
            }
        }

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
            _points = data.MaxHealth;
        }

        #endregion

        private void CalmDown()
        {
            OnCalm?.Invoke();

            Debug.Log($"{name}'s health reached shrinked.".Color(Color.cyan));
        }

        private void Explode()
        {
            Debug.Log($"{name}'s health reached shrinked.".Color(Color.magenta));
            
            foreach (var swallowedObject in swallowedObjects)
            {
                swallowedObject.SetActive(true);
                var rBody = swallowedObject.GetComponent<Rigidbody2D>();

                var vector = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
                rBody.velocity = vector.normalized * 10f;
            }

            Destroy(gameObject);
        }

        public void Swallow(GameObject swallowedObject)
        {
            swallowedObjects.Add(swallowedObject);
            swallowedObject.SetActive(false);
        }

        #endregion
    }
}
