using System.Linq;
using Features.Cards.Indicators;
using UnityEngine;

namespace Features.Cards.InPlay
{
    public class InPlayCardHoverDescriptions : MonoBehaviour
    {
        [SerializeField] private InPlayCard playCard;
        [SerializeField]private EffectDescriptionBox descriptionBoxPrefab;
        [SerializeField]private Transform floatingDescriptionsContainer;
        [SerializeField] private EffectsIconsScriptableObject effectsIconsScriptableObject;
        [SerializeField] private AbilitiesIconsScriptableObject abilityIconsScriptableObject;
        private void OnMouseEnter()
        {
            if (playCard == null)
                return;
            foreach (var effect in playCard.Effects)
            {
                var go = Instantiate(descriptionBoxPrefab, floatingDescriptionsContainer);
                var effectIcon= effectsIconsScriptableObject.effectIcons.FirstOrDefault(x => x.effect == effect.Type);
                if (effectIcon != null) 
                    go.Set(effectIcon.description, effectIcon.image, effect.Amount);
            }
            
            foreach (var ability in playCard.Abilities)
            {
                foreach (var abilityData in ability.AbilityData)
                {
                    var go = Instantiate(descriptionBoxPrefab, floatingDescriptionsContainer);
                    var abilityIcon= abilityIconsScriptableObject.abilitiesIcons.FirstOrDefault(x => x.ability == ability.Type);

                    if (abilityIcon != null) 
                        go.Set(abilityIcon.description, abilityIcon.image, abilityData.Amount);
                }
            }
        }

        private void OnMouseExit()
        {
            foreach (Transform child in floatingDescriptionsContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}