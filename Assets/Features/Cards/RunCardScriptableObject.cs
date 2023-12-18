using System;
using System.Collections.Generic;
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
        public List<Ability> onDealDamageAbilities;
        public List<Ability> onAttackAbilities;
        public List<Ability> onDeadAbilities;
        public List<Ability> onActAbilities;
        public List<Ability> onSpinAbilities;
        public List<Ability> onTurnStartAbilities;
        public List<Ability> onTurnEndAbilities;
        public List<Ability> onBattleStartAbilities;


        // public List<Ability> onBattleEndAbilities;

        public readonly AttackType attackType;
        public int actCost;

        public RunCardScriptableObject(BaseCardScriptableObject cardScriptableObject)
        {
            cardName = cardScriptableObject.cardName;
            hp = cardScriptableObject.hp;
            attack = cardScriptableObject.attack;
            onDealDamageAbilities = new(cardScriptableObject.onDealDamageAbilities?? Array.Empty<Ability>());
            onAttackAbilities = new(cardScriptableObject.onAttackAbilities?? Array.Empty<Ability>());
            onActAbilities = new(cardScriptableObject.onActAbilities?? Array.Empty<Ability>());
            onSpinAbilities = new(cardScriptableObject.onSpinAbilities?? Array.Empty<Ability>());
            onTurnStartAbilities = new(cardScriptableObject.onTurnStartAbilities?? Array.Empty<Ability>());
            onTurnEndAbilities = new(cardScriptableObject.onTurnEndAbilities?? Array.Empty<Ability>());
            onBattleStartAbilities = new(cardScriptableObject.onBattleStartAbilities ?? Array.Empty<Ability>());
            // onBattleEndAbilities = new(cardScriptableObject.onBattleStartAbilities ?? Array.Empty<Ability>());
            onDeadAbilities = new(cardScriptableObject.onDeadAbilities ?? Array.Empty<Ability>()); 
            attackType = cardScriptableObject.attackType;
            actCost = cardScriptableObject.actCost;
            baseCard = cardScriptableObject;
        }
    }
}