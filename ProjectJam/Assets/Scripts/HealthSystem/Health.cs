using UnityEngine;

using BarthaSzabolcs.RichTextHelper;

namespace GameJam.HealthSystem
{
    public class Health : MonoBehaviour
    {
        #region Datamembers

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
                    Die();
                }
                else if (value == 1)
                {
                    _points = value;

                    Shrink();
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

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            _points = data.MaxHealth;
        }

        #endregion

        private void Shrink()
        {
            Debug.Log($"{name}'s health reached shrinked.".Color(Color.cyan));
        }

        private void Die()
        {
            Debug.Log($"{name}'s health reached shrinked.".Color(Color.magenta));
            Destroy(gameObject);
        }

        #endregion
    }
}
