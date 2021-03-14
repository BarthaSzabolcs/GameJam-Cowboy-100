using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using GameJam.FoodGun;

namespace GameJam.UI
{
    public class GunFoodQueueView : MonoBehaviour
    {
        [SerializeField] private GameObject foodViewPrefab;
        [SerializeField] private Gun gun;

        [Header("CurrentFood")]
        [SerializeField] private FoodView currentFood;
        [SerializeField] private Vector3 currentFoodOffset;

        private List<FoodView> views = new List<FoodView>();

        private void Awake()
        {
            gun.OnQueueChanged += Refresh;

            //Cursor.visible = false;
        }

        private void Update()
        {
            currentFood.transform.position = Input.mousePosition + currentFoodOffset;
        }

        private void Refresh(Queue<FoodData> queue)
        {
            if (queue.Count > views.Count)
            {
                while (views.Count != queue.Count)
                {
                    var foodViewInstance = Instantiate(foodViewPrefab, transform);
                    var foodView = foodViewInstance.GetComponent<FoodView>();

                    views.Add(foodView);
                }
            }

            int index = 0;
            foreach (var foodData in queue)
            {
                views[index].Refresh(foodData);
                index++;
            }

            currentFood.Refresh(queue.First());
        }
    }
}
