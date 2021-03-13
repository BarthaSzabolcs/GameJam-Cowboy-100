using UnityEngine;

namespace GameJam.FoodGun
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "Data/Food", order = -1000)]
    public class FoodData : ScriptableObject
    {
        #region Datamembers

        #region Editor Settings

        [field: SerializeField]
        public float Speed { get; private set; }

        [field: SerializeField]
        public int NutriotionValue { get; private set; }

        [field: SerializeField] 
        public string TargetTag { get; set; }

        #endregion

        #endregion
    }
}
