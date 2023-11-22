using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Cards.Indicators
{
    public class IndicatorIconView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text counter;

        public void Set(Sprite icon, int count)
        {
            image.sprite = icon;
            counter.text = $"{count}";
        }
    }
}
