using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Battles.Wheel;
using Features.Cards.Indicators;
using Features.Cards.InPlay.Feedback;
using TMPro;
using UnityEngine;

namespace Features.Cards.InPlay
{
    public class InPlayCard : MonoBehaviour
    {
        private InPlayCardScriptableObject _cardScriptableObject;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite cardBack;
        [SerializeField] private Transform viewContainer;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text atkText;
        [SerializeField] private TMP_Text armorText;
        [SerializeField] private Transform abilitiesContainer;
        [SerializeField] private Transform effectsContainer;
        [SerializeField] private Transform armorContainer;
        [SerializeField] private IndicatorIconView indicatorPrefab;
        [SerializeField] private EffectsIconsScriptableObject effectsIconsScriptableObject;
        [SerializeField] private AbilitiesIconsScriptableObject abilitiesIconsScriptableObject;
        [SerializeField] private InPlayCardFeedbacks inPlayCardFeedbacks;
        [SerializeField] private InPlayCardHoverZoom zoomControl;

        public bool IsDead => _cardScriptableObject.IsDead;

        public List<Effect> Effects => _cardScriptableObject.Effects;

        // public List<Ability> Abilities => _cardScriptableObject.OnDealDamageAbilities
        //     .Concat(_cardScriptableObject.OnAttackAbilities)
        //     .Concat(_cardScriptableObject.OnActAbilities)
        //     .Concat(_cardScriptableObject.OnSpinAbilities)
        //     .Concat(_cardScriptableObject.OnTurnStartAbilities)
        //     .Concat(_cardScriptableObject.OnTurnEndAbilities)
        //     .Concat(_cardScriptableObject.OnDeadAbilities)
        //     .Concat(_cardScriptableObject.OnBattleStartAbilities).ToList();
        public List<Ability> Abilities => _cardScriptableObject.Abilities.ToList();

        public string CardName => _cardScriptableObject.CardName;
        public PlayerController OwnerPlayer;


        public void SetPlayer(bool player)
        {
            if (!player)
                viewContainer.transform.localRotation = new Quaternion(0, 0, 180, 0);
        }

        public void SetCard(InPlayCardScriptableObject runCardScriptableObject, PlayerController owner)
        {
            OwnerPlayer = owner;
            _cardScriptableObject = runCardScriptableObject;
            spriteRenderer.sprite = runCardScriptableObject.CardSprite;

            SetAbilityIcons(Abilities.ToArray());
            SetEffectIcons(Effects.ToArray());

            _cardScriptableObject.ValueChanged += UpdateCardScriptableObjectValues;
            _cardScriptableObject.HealthValueChanged += UpdateHealth;
            _cardScriptableObject.AttackValueChanged += UpdateAttack;
            _cardScriptableObject.ArmorValueChanged += UpdateArmor;
            UpdateCardScriptableObjectValues(null, _cardScriptableObject);
            UpdateHealth(null, (_cardScriptableObject.Health, _cardScriptableObject.Health));
            UpdateAttack(null, (_cardScriptableObject.Attack, _cardScriptableObject.Attack));
            UpdateArmor(null, (_cardScriptableObject.Armor, _cardScriptableObject.Armor));
        }

        private void Awake()
        {
            viewContainer.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _cardScriptableObject.HealthValueChanged -= UpdateHealth;
            _cardScriptableObject.AttackValueChanged -= UpdateAttack;
            _cardScriptableObject.ValueChanged -= UpdateCardScriptableObjectValues;
        }

        private void SetAbilityIcons(Ability[] abilities)
        {
            foreach (Transform child in abilitiesContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var ability in abilities)
            {
                var abilityIcon =
                    abilitiesIconsScriptableObject.abilitiesIcons.FirstOrDefault(x => x.ability == ability.Type);
                var go = Instantiate(indicatorPrefab, abilitiesContainer);
                go.Set(abilityIcon?.image, ability);
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
                var go = Instantiate(indicatorPrefab, effectsContainer);
                go.Set(abilityIcon?.image, effect);
            }

            effectsContainer.gameObject.SetActive(effectsContainer.childCount > 0);
        }

        private void UpdateCardScriptableObjectValues(object sender, InPlayCardScriptableObject e)
        {
            if (e.IsDead)
            {
                hpText.text = "0";
                atkText.text = "0";
                SetEffectIcons(Effects?.ToArray());
            }

            SetEffectIcons(Effects?.ToArray());
        }

        private void UpdateHealth(object sender, (int previous, int current) e) => hpText.text = e.current.ToString();
        private void UpdateAttack(object sender, (int previous, int current) e) => atkText.text = e.current.ToString();

        private void UpdateArmor(object sender, (int previous, int current) e)
        {
            armorText.text = e.current.ToString();
            armorContainer.gameObject.SetActive(e.current > 0);
        }


        public InPlayCardScriptableObject GetCard()
        {
            return _cardScriptableObject;
        }

        public IEnumerator SetDead()
        {
            hpText.text = "0";
            atkText.text = "0";
            _cardScriptableObject.Effects.Clear();
            spriteRenderer.sprite = cardBack;
            UpdateCardScriptableObjectValues(null, _cardScriptableObject);
            yield return inPlayCardFeedbacks.PlayOnDeadFeedback();
        }

        public IEnumerator PlayGetHitAnimation(int damage, AbilityEnum? source = null)
        {
            yield return inPlayCardFeedbacks.PlayOnGetHitFeedback(damage, source);
        }

        public IEnumerator PlayGainShield()
        {
            yield return inPlayCardFeedbacks.PlayOnArmorGain();
        }

        public void UpdateEffect(EffectEnum effectType, int amount)
        {
            if (IsDead) return;
            var effectInCard = Effects.FirstOrDefault(x => x.Type == effectType);
            if (effectInCard == null)
            {
                effectInCard = new Effect();
                Effects.Add(effectInCard);
            }

            effectInCard = new Effect() { Type = effectType, Amount = effectInCard.Amount + amount };

            if (effectInCard.Amount > 0)
                UpdateCardScriptableObjectValues(this, GetCard());
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

        public IEnumerator PlayRangedAttack(InPlayCard defender)
        {
            yield return inPlayCardFeedbacks.PlayOnRangedAttackedFeedback(defender.transform);
        }

        public IEnumerator PlayOnAppearFeedback()
        {
            yield return inPlayCardFeedbacks.PlayOnAppearFeedback();
        }
    }
}