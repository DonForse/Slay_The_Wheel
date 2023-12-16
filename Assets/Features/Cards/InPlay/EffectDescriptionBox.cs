using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Cards.InPlay
{
    public class EffectDescriptionBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image image;

        public void Set(string effectDataDescription, Sprite effectDataImage, int effectAmount)
        {
            var stringFormatted = effectDataDescription
                .Replace("{amount}", $"{effectAmount}")
                .Replace("{_s}", $"{FormatAmount(effectAmount)}");
            description.text = stringFormatted;
            image.sprite = effectDataImage;
        }

        private static string FormatAmount(int effectAmount) => effectAmount == 0 ? "" : "s";
    }
}