using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using MoreMountains.Feedbacks;
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
        [SerializeField] private MMF_Player damageFeedback;
        [SerializeField] private MMF_Player deadFeedback;
        [SerializeField] private MMF_Player showCardFeedback;
        [SerializeField] private Transform container;

        [SerializeField] private GameObject fireIndicatorPrefab;
        [SerializeField] private GameObject turnIndicatorPrefab;
        [SerializeField] private GameObject burnAllIndicatorPrefab;
        [SerializeField] private GameObject atkLeftIndicatorPrefab;
        [SerializeField] private GameObject atkRightIndicatorPrefab;
        [SerializeField] private GameObject addShieldLeftIndicatorPrefab;
        [SerializeField] private GameObject addShieldRightIndicatorPrefab;

        private MMF_FloatingText _feedbackFloatingText;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        public int Shield;
        public int Attack;
        public List<Ability> Effects = new();
        public string CardName => _card.CardName;

        private void OnEnable()
        {
            _feedbackFloatingText = damageFeedback.GetFeedbackOfType<MMF_FloatingText>();
        }

        public void SetPlayer(bool player)
        {
            if (!player)
                viewContainer.transform.localRotation = new Quaternion(0, 0, 180, 0);
        }

        public IEnumerator SetCard(RunCard runCard)
        {
            _card = runCard;
            _isDead = false;
            
            Attack = runCard.Attack;
            Shield = 0;
            
            spriteRenderer.sprite = runCard.baseCard.cardSprite;

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            SetEffectIcons(runCard);

            _card.ValueChanged += UpdateCardValues;
            UpdateCardValues(null, _card);
            yield return showCardFeedback.PlayFeedbacksCoroutine(this.transform.position);
        }

        private void SetEffectIcons(RunCard runCard)
        {
            var burns = runCard.baseCard.abilities.Count(x => x == Ability.Burn);
            for (int i = 0; i < burns; i++)
                Instantiate(fireIndicatorPrefab, container);

            var rotateR = runCard.baseCard.abilities.Count(x => x == Ability.RotateRight);
            for (int i = 0; i < rotateR; i++)
                Instantiate(turnIndicatorPrefab, container);

            var rotateL = runCard.baseCard.abilities.Count(x => x == Ability.RotateLeft);
            for (int i = 0; i < rotateL; i++)
                Instantiate(turnIndicatorPrefab, container);
            
            var burnAll = runCard.baseCard.abilities.Count(x => x == Ability.BurnAll);
            for (int i = 0; i < burnAll; i++)
                Instantiate(burnAllIndicatorPrefab, container);
            
            var addAtkLeft = runCard.baseCard.abilities.Count(x => x == Ability.AddAtkLeft);
            for (int i = 0; i < addAtkLeft; i++)
                Instantiate(atkLeftIndicatorPrefab, container);
            var addAtkRight = runCard.baseCard.abilities.Count(x => x == Ability.AddAtkRight);
            for (int i = 0; i < addAtkRight; i++)
                Instantiate(atkRightIndicatorPrefab, container);
            var addShieldLeft = runCard.baseCard.abilities.Count(x => x == Ability.AddShieldLeft);
            for (int i = 0; i < addShieldLeft; i++)
                Instantiate(addShieldLeftIndicatorPrefab, container);
            var addShieldRight = runCard.baseCard.abilities.Count(x => x == Ability.AddShieldRight);
            for (int i = 0; i < addShieldRight; i++)
                Instantiate(addShieldRightIndicatorPrefab, container);

        }

        private void UpdateCardValues(object sender, RunCard e)
        {
            if (_isDead)
            {
                hpText.text = "0";
                atkText.text = "0";
            }

            hpText.text = e.Hp.ToString();
            atkText.text =Attack.ToString();
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
            Effects.Clear();
            spriteRenderer.sprite = cardBack;
            return deadFeedback.PlayFeedbacksCoroutine(this.transform.position);
        }

        public IEnumerator PlayGetHitAnimation(int damage, Ability? source = null)
        {
            _feedbackFloatingText.Value = damage.ToString();
            SetAnimationTextColor(source);

            // _player.PlayFeedbacks(this.transform.position, damage);
            return damageFeedback.PlayFeedbacksCoroutine(this.transform.position, damage);
        }

        private void SetAnimationTextColor(Ability? source)
        {
            switch (source)
            {
                case null:
                    SetNormalColor();
                    break;
                case Ability.Burn:
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
    }
}