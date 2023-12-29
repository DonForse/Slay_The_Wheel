using System.Collections;
using System.Collections.Generic;
using Features.Battles.Core.Abilities;
using Features.Cards;

namespace Features.Battles.Managers
{
    public class BattleAbilitiesManager
    {
        public BattleAbilitiesManager(Battle battle)
        {
            _battle = battle;
            _abilities = new()
            {
                new AddVulnerableOnApplyAbilityStrategy(),
                new AddBurnOnApplyAbilityStrategy(),
                new AddOilOnApplyAbilityStrategy(),
                new AddBombOnApplyAbilityStrategy(),
                new RotateLeftOnApplyAbilityStrategy(),
                new RotateRightOnApplyAbilityStrategy(),
                new DealAttackDamageOnApplyAbilityStrategy(_battle),
                new MultiAttackOnApplyAbilityStrategy(_battle),
                new AddShieldOnApplyAbilityStrategy(),
                new AddAttackOnApplyAbilityStrategy()
            };
        }

        private readonly List<IOnApplyAbilityStrategy> _abilities;
        private readonly Battle _battle;

        public IEnumerator ApplyAbilitiesEffect(BattleEventEnum battleEventEnum)
        {
            foreach (var card in _battle.playerController.Cards)
            {
                foreach (var cardAbility in card.Abilities)
                {
                    foreach (var ability in _abilities)
                    {
                        if (battleEventEnum == cardAbility.battleEventEnum && ability.IsValid(cardAbility.Type))
                        {
                            yield return ability.Execute(cardAbility, card, _battle.enemyController, _battle.playerController);
                        }
                    }
                }
            }
            foreach (var card in _battle.enemyController.Cards)
            {
                foreach (var cardAbility in card.Abilities)
                {
                    foreach (var ability in _abilities)
                    {
                        if (battleEventEnum == cardAbility.battleEventEnum && ability.IsValid(cardAbility.Type))
                        {
                            yield return ability.Execute(cardAbility, card, _battle.playerController, _battle.enemyController);
                        }
                    }
                }
            }
            yield break;
        }
    }
}