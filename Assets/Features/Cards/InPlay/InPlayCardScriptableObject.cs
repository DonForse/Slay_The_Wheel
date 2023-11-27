using System;
using Features.Battles;
using UnityEngine;

namespace Features.Cards.InPlay
{
    public class InPlayCardScriptableObject : ScriptableObject
    {
        public event EventHandler<(int previous, int current)> HealthValueChanged;
        public event EventHandler<(int previous, int current)> AttackValueChanged;
        public event EventHandler<InPlayCardScriptableObject> ValueChanged;

        public string CardName => _cardName;
        public Sprite CardSprite => _runCard.baseCard.cardSprite;

        public int Health
        {
            get => _runCard.hp;
            set
            {
                var previous = _runCard.hp;
                _runCard.hp = value;
                HealthValueChanged?.Invoke(this, (previous,_runCard.hp));
            }
        }

        public int Attack
        {
            get => _attack;
            set
            {
                var previous = _attack;

                _attack = value;
                AttackValueChanged?.Invoke(this, (previous, _attack));
            }
        }
        public Ability[] OnDealDamageAbilities
        {
            get => _onDealDamageAbilities;
            set
            {
                _onDealDamageAbilities = value;
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

        public bool IsDead => _runCard.hp <= 0;

        public AttackType AttackType
        {
            get => _attackType;

            set
            {
                _attackType = value;
                ValueChanged?.Invoke(this, this);
            }
        }

        public int ActCost { get => _actCost;

            set
            {
                _actCost = value;
                ValueChanged?.Invoke(this, this);
            } }


        private string _cardName;
        private int _attack;
        private Ability[] _onAttackAbilities;
        private Ability[] _onDealDamageAbilities;
        private Ability[] _onSpinAbilities;
        private Ability[] _onTurnStartAbilities;
        private Ability[] _onTurnEndAbilities;
        private Ability[] _onActAbilities;
        private AttackType _attackType;
        private Sprite _cardSprite;
        private int _actCost;
        private readonly RunCardScriptableObject _runCard;

        public InPlayCardScriptableObject(RunCardScriptableObject cardScriptableObject)
        {
            _cardName = cardScriptableObject.cardName;
            _attack = cardScriptableObject.attack;
            _onDealDamageAbilities = cardScriptableObject.onDealDamageAbilities;
            _onAttackAbilities = cardScriptableObject.onAttackAbilities;
            _onActAbilities = cardScriptableObject.onActAbilities;
            _onSpinAbilities = cardScriptableObject.onSpinAbilities;
            _onTurnStartAbilities = cardScriptableObject.onTurnStartAbilities;
            _onTurnEndAbilities = cardScriptableObject.onTurnEndAbilities;
            _attackType = cardScriptableObject.attackType;
            _actCost = cardScriptableObject.actCost;
            _runCard = cardScriptableObject;

        }
    }
}