using Features.Cards.InPlay;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public class ZoomWheel :MonoBehaviour
    {
        [SerializeField] protected PlayerController playerController;

        private void OnEnable()
        {
            playerController.ActiveCardChanged += OnActiveCardChanged;
        }

        private void OnActiveCardChanged(object sender, InPlayCard e)
        {
            foreach (var card in playerController.Cards)
            {
                // card.SetAsActive(card == e);
            }
        }

        public void SetValue(InPlayCard card,float scale)
        {
            card.SetScale(scale);
        }
    }
}