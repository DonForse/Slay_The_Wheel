using UnityEngine;

namespace Features.Cards.InPlay
{
    public class InPlayCardHoverDescriptions : MonoBehaviour
    {
        [SerializeField] private InPlayCard playCard;
        [SerializeField]private EffectDescriptionBox descriptionBoxPrefab;
        [SerializeField]private Transform floatingDescriptionsContainer;

        private void OnMouseEnter()
        {
            if (playCard == null)
                return;
            foreach (var effect in playCard.Effects)
            {
                Instantiate(descriptionBoxPrefab, floatingDescriptionsContainer);
            }
            
            foreach (var effect in playCard.Abilities)
            {
            }
        }

        private void OnMouseExit()
        {
            if (playCard == null)
                return;
        }
    }
}