using System.Collections.Generic;
using UnityEngine;

namespace GameJam.FoodGun
{
    public class Gun : MonoBehaviour
    {
        #region Datamembers

        #region Events

        public delegate void QueueChanged(Queue<FoodData> queue);
        public event QueueChanged OnQueueChanged;

        #endregion
        #region Editor Settings

        [SerializeField] private GameObject foodPrefab;
        [SerializeField] private FoodData[] foods;

        [SerializeField] private Vector2 barrelOffset;

        [SerializeField] private SpriteRenderer leftHand;
        [SerializeField] private SpriteRenderer rightHand;

        #endregion
        #region Private Properties

        private Vector2 BarrelPosition
        {
            get
            {
                return transform.TransformPoint(barrelOffset);
            }
        }

        #endregion
        #region Private Fields

        private Queue<FoodData> foodQueue = new Queue<FoodData>();
        private SpriteRenderer spriteRenderer;

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
                LoadFood();
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            transform.localScale = new Vector3(x: 1, y: transform.right.x > 0 ? 1 : -1, z: 0);

            if (transform.right.y < 0)
            {
                spriteRenderer.sortingOrder = 1;
                leftHand.sortingOrder = 2;
                rightHand.sortingOrder = 3;
            }
            else
            {
                spriteRenderer.sortingOrder = -1;
                leftHand.sortingOrder = -2;
                rightHand.sortingOrder = -3;
            }
        }

        #endregion

        public void Shoot()
        {
            var instance = Instantiate(foodPrefab, BarrelPosition, Quaternion.identity);
            instance.transform.right = transform.right;

            instance.GetComponent<Food>().Data = foodQueue.Dequeue();
            
            LoadFood();
        }

        private void LoadFood()
        {
            foodQueue.Enqueue(foods[Random.Range(0, foods.Length)]);

            OnQueueChanged?.Invoke(foodQueue);
        }

        #endregion
    }
}