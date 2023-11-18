using System;
using System.Collections.Generic;
using Features.Battles;

namespace Features.Cards
{
    public class RunCard
    {
        public string CardName
        {
            get => _cardName;
            set
            {
                _cardName = value;
                ValueChanged?.Invoke(this, this);
            }
        }

        public int Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                ValueChanged?.Invoke(this, this);
            }
        }

        public int Attack
        {
            get => _attack;
            set
            {
                _attack = value;
                ValueChanged?.Invoke(this, this);
            }
        }
    
        public Ability[] OnHitAbilities
        {
            get => _onHitAbilities;
            set
            {
                _onHitAbilities = value;
                ValueChanged?.Invoke(this, this);
            }
        }

        public Ability[] OnAttackAbilities
        {
            get => _onAttackAbilities;
            set
            {
                _onAttackAbilities = value;
                ValueChanged?.Invoke(this, this);
            }
        }
        public Ability[] OnActAbilities
        {
            get => _onActAbilities;
            set
            {
                _onActAbilities = value;
                ValueChanged?.Invoke(this, this);
            }
        }
        public Ability[] OnSpinAbilities
        {
            get => _onSpinAbilities;
            set
            {
                _onSpinAbilities = value;
                ValueChanged?.Invoke(this, this);
            }
        }
        public Ability[] OnTurnStartAbilities
        {
            get => _onTurnStartAbilities;
            set
            {
                _onTurnStartAbilities = value;
                ValueChanged?.Invoke(this, this);
            }
        }
        public Ability[] OnTurnEndAbilities
        {
            get => _onTurnEndAbilities;
            set
            {
                _onTurnEndAbilities = value;
                ValueChanged?.Invoke(this, this);
            }
        }
        
        public AttackType AttackType { get; set; }
        public bool IsDead => Hp <= 0;
        public int ActCost => baseCard.actCost;


        public readonly BaseCardScriptableObject baseCard;
    
        private int _attack;
        private Ability[] _onHitAbilities;
        private Ability[] _onAttackAbilities;
        private Ability[] _onActAbilities;
        private int _hp;
        private string _cardName;
        private Ability[] _onSpinAbilities;
        private Ability[] _onTurnStartAbilities;
        private Ability[] _onTurnEndAbilities;

        public RunCard(BaseCardScriptableObject cardScriptableObject)
        {
            _cardName = cardScriptableObject.cardName;
            _hp = cardScriptableObject.hp;
            _attack = cardScriptableObject.attack;
            _onHitAbilities = cardScriptableObject.onHitAbilities;
            _onAttackAbilities = cardScriptableObject.onAttackAbilities; 
            _onActAbilities = cardScriptableObject.onActAbilities;
            _onSpinAbilities = cardScriptableObject.onSpinAbilities;
            _onTurnStartAbilities = cardScriptableObject.onTurnStartAbilities;
            _onTurnEndAbilities = cardScriptableObject.onTurnEndAbilities;

            baseCard = cardScriptableObject;
            AttackType = cardScriptableObject.attackType;
        }

        public event EventHandler<RunCard> ValueChanged;
    }
}