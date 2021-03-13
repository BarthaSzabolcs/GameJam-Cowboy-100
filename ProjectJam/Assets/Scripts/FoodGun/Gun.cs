using System.Collections.Generic;
using UnityEngine;

namespace GameJam.FoodGun
{
    public class Gun : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [SerializeField] private GameObject[] foods;

        [SerializeField] private Vector3 barrelOffset;

        #endregion
        #region Private Properties

        private Vector3 BarrelPosition
        {
            get
            {
                return transform.TransformPoint(barrelOffset);
            }
        }

        #endregion
        #region Private Fields

        private Queue<GameObject> foodQueue = new Queue<GameObject>();

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(BarrelPosition, 0.125f);
        }

        private void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                foodQueue.Enqueue(NextFood());
            }
        }

        #endregion

        public void Shoot()
        {
            var food = foodQueue.Dequeue();

            var instance = Instantiate(food, BarrelPosition, Quaternion.identity);
            instance.transform.forward = transform.forward;

            foodQueue.Enqueue(NextFood());
        }

        private GameObject NextFood()
        {
            return foods[Random.Range(0, foods.Length)];
        }

        #endregion
    }
}