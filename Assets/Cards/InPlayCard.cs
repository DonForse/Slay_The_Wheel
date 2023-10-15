using System;
using System.Linq;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class InPlayCard : MonoBehaviour
{
    private RunCard _card;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite cardBack;
    [SerializeField] private Transform viewContainer;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private MMF_Player _player;
    private MMF_FloatingText _feedbackFloatingText;
    private bool _isDead = false;
    public string CardName => _card.CardName;

    private void OnEnable()
    {
        _feedbackFloatingText = _player.GetFeedbackOfType<MMF_FloatingText>();
    }

    public void SetCard(RunCard runCard, bool player)
    {
        _card = runCard;
        spriteRenderer.sprite = runCard.baseCard.cardSprite;
        if (!player)
            viewContainer.transform.localRotation = new Quaternion(0, 0, 180, 0);

        _card.ValueChanged += UpdateCardValues;
        UpdateCardValues(null, _card);
    }

    private void UpdateCardValues(object sender, RunCard e)
    {
        if (_isDead)
        {
            hpText.text = "0";
            atkText.text = "0";
        }

        hpText.text = e.Hp.ToString();
        atkText.text = e.Attack.ToString();
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
        spriteRenderer.sprite = cardBack;
    }

    public void PlayGetHitAnimation(int damage, Ability? source = null)
    {
        _feedbackFloatingText.Value = damage.ToString();
        SetAnimationTextColor(source);

        _player.PlayFeedbacks(this.transform.position, damage);
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

    public bool IsDead => _isDead;
}