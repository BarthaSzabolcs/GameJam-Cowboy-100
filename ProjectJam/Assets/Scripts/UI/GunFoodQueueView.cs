using System.Collections.Generic;
using UnityEngine;

using GameJam.FoodGun;

namespace GameJam.UI
{
    public class GunFoodQueueView : MonoBehaviour
    {
        [SerializeField] private GameObject foodViewPrefab;
        [SerializeField] private Gun gun;

        private List<FoodView> views = new List<FoodView>();

        private void Awake()
        {
            gun.OnQueueChanged += Refresh;
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
        }
    }
}
