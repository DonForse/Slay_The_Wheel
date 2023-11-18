using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Battles.Wheel;
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
        [SerializeField] private GameObject fireIndicatorPrefab;
        [SerializeField] private GameObject turnIndicatorPrefab;
        [SerializeField] private GameObject burnAllIndicatorPrefab;
        [SerializeField] private GameObject atkLeftIndicatorPrefab;
        [SerializeField] private GameObject atkRightIndicatorPrefab;
        [SerializeField] private GameObject addShieldLeftIndicatorPrefab;
        [SerializeField] private GameObject addShieldRightIndicatorPrefab;
        [SerializeField] private Animator animator;
        private MMF_FloatingText _feedbackFloatingText;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        public int Armor;
        public int Attack;

        public List<Ability> Effects = new List<Ability>();

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

            SetEffectIcons(runCard.OnDealDamageAbilities, abilitiesContainer);
            SetEffectIcons(Effects.ToArray(), effectsContainer);

            _card.ValueChanged += UpdateCardValues;
            UpdateCardValues(null, _card);
            yield return showCardFeedback.PlayFeedbacksCoroutine(this.transform.position);
        }

        private void SetEffectIcons(Ability[] abilities, Transform effectIconContainer)
        {
            if (effectIconContainer != null)
            {
                foreach (Transform child in effectIconContainer)
                {
                    Destroy(child.gameObject);
                }
            }

            var burns = abilities.FirstOrDefault(x => x.Type == AbilityEnum.Burn);
            if (burns != null)
            {
                for (int i = 0; i < burns.Amount; i++)
                    Instantiate(fireIndicatorPrefab, effectIconContainer);
            }


            var rotateR = abilities.FirstOrDefault(x => x.Type == AbilityEnum.RotateRight);
            if (rotateR != null)
                for (int i = 0; i < rotateR.Amount; i++)
                    Instantiate(turnIndicatorPrefab, effectIconContainer);

            var rotateL = abilities.FirstOrDefault(x => x.Type == AbilityEnum.RotateLeft);
            if (rotateL != null)
                for (int i = 0; i < rotateL.Amount; i++)
                    Instantiate(turnIndicatorPrefab, effectIconContainer);

            var burnAll = abilities.FirstOrDefault(x => x.Type == AbilityEnum.BurnAll);
            if (burnAll != null)
                for (int i = 0; i < burnAll.Amount; i++)
                    Instantiate(burnAllIndicatorPrefab, effectIconContainer);

            var addAtkLeft = abilities.FirstOrDefault(x => x.Type == AbilityEnum.AddAtkLeft);
            if (addAtkLeft != null)
                for (int i = 0; i < addAtkLeft.Amount; i++)
                    Instantiate(atkLeftIndicatorPrefab, effectIconContainer);

            var addAtkRight = abilities.FirstOrDefault(x => x.Type == AbilityEnum.AddAtkRight);
            if (addAtkRight != null)
                for (int i = 0; i < addAtkRight.Amount; i++)
                    Instantiate(atkRightIndicatorPrefab, effectIconContainer);

            var addShieldLeft = abilities.FirstOrDefault(x => x.Type == AbilityEnum.AddShieldLeft);
            if (addShieldLeft != null)
                for (int i = 0; i < addShieldLeft.Amount; i++)
                    Instantiate(addShieldLeftIndicatorPrefab, effectIconContainer);

            var addShieldRight = abilities.FirstOrDefault(x => x.Type == AbilityEnum.AddShieldRight);
            if (addShieldRight != null)
                for (int i = 0; i < addShieldRight.Amount; i++)
                    Instantiate(addShieldRightIndicatorPrefab, effectIconContainer);

            if (effectIconContainer != null)
            {
                effectIconContainer.gameObject.SetActive(effectIconContainer.childCount > 0);
            }
        }

        private void UpdateCardValues(object sender, RunCard e)
        {
            if (_isDead)
            {
                hpText.text = "0";
                atkText.text = "0";
                SetEffectIcons(Effects?.ToArray(), effectsContainer);
            }

            SetEffectIcons(Effects?.ToArray(), effectsContainer);

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
            Effects = new List<Ability>();
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

        public void UpdateEffect(AbilityEnum effectType, int amount)
        {
            if (IsDead) return;
            var effectInCard = Effects.FirstOrDefault(x => x.Type == effectType);
            if (effectInCard != null)
                Effects.Remove(effectInCard);
            else
                effectInCard = new Ability();

            effectInCard = new Ability() { Type = effectType, Amount = effectInCard.Amount + amount };
            
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