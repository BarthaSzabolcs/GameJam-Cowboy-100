using UnityEngine;
using UnityEngine.UI;

using TMPro;

using GameJam.FoodGun;

namespace GameJam.UI
{
    public class FoodView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nutritionalValue;

        public void Refresh(FoodData data)
        {
            image.sprite = data.Sprite;
            nutritionalValue.text = data.NutriotionValue.ToString();
        }
    }
}
