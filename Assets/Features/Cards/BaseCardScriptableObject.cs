using System.Collections.Generic;
using Features.Battles;
using UnityEngine;

namespace Features.Cards
{
    [CreateAssetMenu(fileName = "Card", menuName = "Cards/Base Card")]
    public class BaseCardScriptableObject : ScriptableObject
    {
        public string cardName;
        public int hp;
        public int attack;
        public Ability[] onAttackAbilities;
        public Ability[] onDealDamageAbilities;
        public Ability[] onSpinAbilities;
        public Ability[] onTurnStartAbilities;
        public Ability[] onTurnEndAbilities;
        public Ability[] onActAbilities;
        public Ability[] onBattleStartAbilities;
        public Ability[] onDeadAbilities;
        public AttackType attackType;
        public Sprite cardSprite;
        public int actCost;
        public int goldCost;
    }
}