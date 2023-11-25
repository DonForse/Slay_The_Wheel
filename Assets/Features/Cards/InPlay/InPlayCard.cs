using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Battles.Wheel;
using Features.Cards.Indicators;
using Features.Cards.InPlay;
using Features.Cards.InPlay.Feedback;
using TMPro;
using UnityEngine;

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
        [SerializeField] private Transform abilitiesContainer;
        [SerializeField] private Transform effectsContainer;
        [SerializeField] private IndicatorIconView indicatorPrefab;
        [SerializeField] private EffectsIconsScriptableObject effectsIconsScriptableObject;
        [SerializeField] private AbilitiesIconsScriptableObject abilitiesIconsScriptableObject;
        [SerializeField] private InPlayCardFeedbacks inPlayCardFeedbacks;
        [SerializeField] private InPlayCardHoverZoom zoomControl;
        
        private bool _isDead = false;

        public bool IsDead => _isDead;

        public int Armor;
        public int Attack;

        public List<Effect> Effects = new();

        public string CardName => _card.CardName;
        public PlayerController OwnerPlayer;


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
            yield return inPlayCardFeedbacks.PlayOnAppearFeedback();
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

        public IEnumerator SetDead()
        {
            _isDead = true;
            hpText.text = "0";
            atkText.text = "0";
            Effects = new List<Effect>();
            spriteRenderer.sprite = cardBack;
            yield return inPlayCardFeedbacks.PlayOnDeadFeedback();
            
        }

        public IEnumerator PlayGetHitAnimation(int damage, AbilityEnum? source = null)
        {
            yield return inPlayCardFeedbacks.PlayOnGetHitFeedback(damage, source);
        }

        public IEnumerator PlayGainArmor()
        {
            yield return inPlayCardFeedbacks.PlayOnArmorGain();
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

        public IEnumerator PlayAttack()
        {
            if (IsDead) yield break;
            yield return inPlayCardFeedbacks.PlayOnAttackFeedback();
        }

        public void SetAsActive(bool value)
        {
            zoomControl.SetActive(value);
        }

        public void SetScale(float scale)
        {
            zoomControl.SetScale(scale);
        }
    }
}