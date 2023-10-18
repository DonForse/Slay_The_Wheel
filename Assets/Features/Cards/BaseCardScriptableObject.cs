using Features.Battle;
using UnityEngine;

namespace Features.Cards
{
    [CreateAssetMenu(fileName = "Card", menuName = "Cards/Base Card")]
    public class BaseCardScriptableObject : ScriptableObject
    {
        public string cardName;
        public int hp;
        public int attack;
        public Ability[] abilities;
        public Sprite cardSprite;
        public AttackType attackType;
    }
}