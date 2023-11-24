using System;
using System.Collections;
using Features.Battles;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Features.Cards.InPlay.Feedback
{
    public class InPlayCardFeedbacks : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private MMF_Player damageFeedback;
        [SerializeField] private MMF_Player deadFeedback;
        [SerializeField] private MMF_Player showCardFeedback;
        [SerializeField] private MMF_Player atkCardFeedback;
        [SerializeField] private MMF_Player gainArmorFeedback;
        private MMF_FloatingText _feedbackFloatingText;

        private void OnEnable()
        {
            _feedbackFloatingText = damageFeedback.GetFeedbackOfType<MMF_FloatingText>();
        }

        public IEnumerator PlayOnArmorGain()
        {
            yield return gainArmorFeedback.PlayFeedbacksCoroutine(this.transform.position);
            
        }

        public IEnumerator PlayOnAppearFeedback()
        {
            yield return showCardFeedback.PlayFeedbacksCoroutine(this.transform.position);
        }

        public IEnumerator PlayOnDeadFeedback()
        {
            yield return deadFeedback.PlayFeedbacksCoroutine(this.transform.position);
        }

        public IEnumerator PlayOnGetHitFeedback(int damage, AbilityEnum? source)
        {
            _feedbackFloatingText.Value = damage.ToString();
            animator.SetTrigger($"ReceiveDamage{source?.ToString() ?? string.Empty}");
            SetAnimationTextColor(source);

            // _player.PlayFeedbacks(this.transform.position, damage);
            yield return damageFeedback.PlayFeedbacksCoroutine(this.transform.position, damage);

            void SetAnimationTextColor(AbilityEnum? source)
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

            void SetCommonColor()
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

            void SetFireColor()
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

            void SetNormalColor()
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

        public IEnumerator PlayOnAttackFeedback()
        {
            yield return atkCardFeedback.PlayFeedbacksCoroutine(this.transform.position);
        }
    }
}