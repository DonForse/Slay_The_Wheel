using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Battles.Wheel;
using Features.Cards.Indicators;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Cards
{
    public class InPlayCard : MonoBehaviour
    {
        private RunCard _card;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite cardBack;
        [SerializeField] private Transform viewContainer;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text atkText;
        [SerializeField] private MMF_Player damageFeedback;
        [SerializeField] private MMF_Player deadFeedback;
        [SerializeField] private MMF_Player showCardFeedback;
        [SerializeField] private MMF_Player atkCardFeedback;
        [SerializeField] private Transform abilitiesContainer;
        [SerializeField] private Transform effectsContainer;
        [SerializeField] private IndicatorIconView indicatorPrefab;
        [SerializeField] private EffectsIconsScriptableObject effectsIconsScriptableObject;
        [SerializeField] private AbilitiesIconsScriptableObject abilitiesIconsScriptableObject;
        [SerializeField] private Animator animator;
        private MMF_FloatingText _feedbackFloatingText;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        public int Armor;
        public int Attack;

        public List<Effect> Effects = new();

        public string CardName => _card.CardName;
        public PlayerController OwnerPlayer;

        private void OnEnable()
        {
            _feedbackFloatingText = damageFeedback.GetFeedbackOfType<MMF_FloatingText>();
        }

        public void SetPlayer(bool player)
        {
            if (!player)
                viewContainer.transform.localRotation = new Quaternion(0, 0, 180, 0);
        }

        public IEnumerator SetCard(RunCard runCard, PlayerController owner)
        {
            OwnerPlayer = owner;
            _card = runCard;
            _isDead = false;

            Attack = runCard.Attack;
            Armor = 0;

            spriteRenderer.sprite = runCard.baseCard.cardSprite;

            SetAbilityIcons(
                runCard.OnDealDamageAbilities
                    .Concat(runCard.OnAttackAbilities)
                    .Concat(runCard.OnActAbilities)
                    .Concat(runCard.OnSpinAbilities)
                    .Concat(runCard.OnTurnStartAbilities)
                    .Concat(runCard.OnTurnEndAbilities).ToArray());
            SetEffectIcons(Effects.ToArray());

            _card.ValueChanged += UpdateCardValues;
            UpdateCardValues(null, _card);
            yield return showCardFeedback.PlayFeedbacksCoroutine(this.transform.position);
        }

        private void OnDestroy()
        {
            _card.ValueChanged -= UpdateCardValues;
        }

        private void SetAbilityIcons(Ability[] abilities)
        {
            foreach (Transform child in abilitiesContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var ability in abilities)
            {
                var abilityIcon = abilitiesIconsScriptableObject.abilitiesIcons.FirstOrDefault(x => x.ability == ability.Type);
                var go  =Instantiate(indicatorPrefab, abilitiesContainer);
                go.Set(abilityIcon?.image, ability.Amount);
            }
            
            abilitiesContainer.gameObject.SetActive(abilitiesContainer.childCount > 0);
        }

        private void SetEffectIcons(Effect[] effects)
        {
            foreach (Transform child in effectsContainer)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var effect in effects)
            {
                var abilityIcon = effectsIconsScriptableObject.effectIcons.FirstOrDefault(x => x.effect == effect.Type);
                var go  =Instantiate(indicatorPrefab, effectsContainer);
                go.Set(abilityIcon?.image, effect.Amount);
            }
           
            effectsContainer.gameObject.SetActive(effectsContainer.childCount > 0);
        }

        private void UpdateCardValues(object sender, RunCard e)
        {
            if (_isDead)
            {
                hpText.text = "0";
                atkText.text = "0";
                SetEffectIcons(Effects?.ToArray());
            }

            SetEffectIcons(Effects?.ToArray());

            hpText.text = e.Hp.ToString();
            atkText.text = Attack.ToString();
        }

        public RunCard GetCard()
        {
            return _card;
        }

        public void SetDead()
        {
            _isDead = true;
            hpText.text = "0";
            atkText.text = "0";
            Effects = new List<Effect>();
            spriteRenderer.sprite = cardBack;
            deadFeedback.PlayFeedbacks(this.transform.position);
        }

        public void PlayGetHitAnimation(int damage, AbilityEnum? source = null)
        {
            _feedbackFloatingText.Value = damage.ToString();
            animator.SetTrigger($"ReceiveDamage{source?.ToString() ?? string.Empty}");
            SetAnimationTextColor(source);

            // _player.PlayFeedbacks(this.transform.position, damage);
            damageFeedback.PlayFeedbacks(this.transform.position, damage);
        }

        private void SetAnimationTextColor(AbilityEnum? source)
        {
            switch (source)
            {
                case null:
                    SetNormalColor();
                    break;
                case AbilityEnum.Burn:
                    SetFireColor();
                    break;
                default:
                    SetCommonColor();
                    break;
            }
        }

        private void SetCommonColor()
        {
            _feedbackFloatingText.ForceColor = true;
            _feedbackFloatingText.AnimateColorGradient = new Gradient()
            {
                mode = GradientMode.Fixed,
                colorKeys = new[]
                {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                }
            };
        }

        private void SetFireColor()
        {
            _feedbackFloatingText.ForceColor = true;
            _feedbackFloatingText.AnimateColorGradient = new Gradient()
            {
                mode = GradientMode.Fixed,
                colorKeys = new[]
                {
                    new GradientColorKey(Color.red, 0f),
                    new GradientColorKey(Color.red, 1f)
                }
            };
        }

        private void SetNormalColor()
        {
            _feedbackFloatingText.ForceColor = true;
            _feedbackFloatingText.AnimateColorGradient = new Gradient()
            {
                mode = GradientMode.Fixed,
                colorKeys = new[]
                {
                    new GradientColorKey(Color.yellow, 0f),
                    new GradientColorKey(Color.yellow, 1f)
                }
            };
        }

        public void UpdateEffect(EffectEnum effectType, int amount)
        {
            if (IsDead) return;
            var effectInCard = Effects.FirstOrDefault(x => x.Type == effectType);
            if (effectInCard != null)
                Effects.Remove(effectInCard);
            else
                effectInCard = new Effect();

            effectInCard = new Effect() { Type = effectType, Amount = effectInCard.Amount + amount };

            if (effectInCard.Amount > 0)
                Effects.Add(effectInCard);

            UpdateCardValues(this, GetCard());
        }

        public void PlayAct()
        {
            if (IsDead) return;
            atkCardFeedback.PlayFeedbacks();
        }
    }
}