using UnityEngine;

namespace GameJam.HealthSystem
{
    [CreateAssetMenu(fileName ="HealthData", menuName ="Data/Health", order =-1000)]
    public class HealthData : ScriptableObject
    {
        #region Datamembers

        #region Editor Settings

        [field: SerializeField] 
        public int MaxHealth { get; private set; }

        #endregion

        #endregion
    }
}
