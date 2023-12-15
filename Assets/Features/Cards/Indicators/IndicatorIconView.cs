using Features.Battles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Cards.Indicators
{
    public class IndicatorIconView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text counter;

        public void Set(Sprite icon, Ability ability)
        {
            image.sprite = icon;
            counter.text = $"{100}";
        }

        public void Set(Sprite icon, Effect ability)
        {
            image.sprite = icon;
            counter.text = $"{100}";
        }
    }
}
