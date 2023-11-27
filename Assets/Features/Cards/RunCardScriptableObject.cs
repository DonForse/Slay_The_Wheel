using Features.Battles;
using UnityEngine;

namespace Features.Cards
{
    public class RunCardScriptableObject : ScriptableObject
    {
        public readonly BaseCardScriptableObject baseCard;

        public string cardName;
        public int hp;
        public int attack;
        public Ability[] onDealDamageAbilities;
        public Ability[] onAttackAbilities;
        public Ability[] onActAbilities;
        public Ability[] onSpinAbilities;
        public Ability[] onTurnStartAbilities;
        public Ability[] onTurnEndAbilities;

        public readonly AttackType attackType;
        public int actCost;

        public RunCardScriptableObject(BaseCardScriptableObject cardScriptableObject)
        {
            cardName = cardScriptableObject.cardName;
            hp = cardScriptableObject.hp;
            attack = cardScriptableObject.attack;
            onDealDamageAbilities = cardScriptableObject.onDealDamageAbilities;
            onAttackAbilities = cardScriptableObject.onAttackAbilities;
            onActAbilities = cardScriptableObject.onActAbilities;
            onSpinAbilities = cardScriptableObject.onSpinAbilities;
            onTurnStartAbilities = cardScriptableObject.onTurnStartAbilities;
            onTurnEndAbilities = cardScriptableObject.onTurnEndAbilities;
            attackType = cardScriptableObject.attackType;
            actCost = cardScriptableObject.actCost;
            baseCard = cardScriptableObject;
        }
    }
}