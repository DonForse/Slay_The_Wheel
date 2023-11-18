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
        
        public AttackType AttackType { get; set; }
        public bool IsDead => Hp <= 0;
        public int ActCost => baseCard.actCost;


        public readonly BaseCardScriptableObject baseCard;
    
        private int _attack;
        private Ability[] _onHitAbilities;
        private Ability[] _onAttackAbilities;
        private int _hp;
        private string _cardName;

        public RunCard(BaseCardScriptableObject heroCardDb)
        {
            _cardName = heroCardDb.cardName;
            _hp = heroCardDb.hp;
            _attack = heroCardDb.attack;
            _onHitAbilities = heroCardDb.onHitAbilities;
            _onAttackAbilities = heroCardDb.onAttackAbilities;
            baseCard = heroCardDb;
            AttackType = heroCardDb.attackType;
        }

        public event EventHandler<RunCard> ValueChanged;
    }
}