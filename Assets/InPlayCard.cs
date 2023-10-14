using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
 
public class InPlayCard : MonoBehaviour
{
    private RunCard _card;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform viewContainer;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text atkText;
    private bool _isDead = false;
    public string CardName => _card.CardName;
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
        spriteRenderer.sprite =
            Sprite.Create(Texture2D.grayTexture, new Rect(Vector2.zero, Vector2.one), Vector2.one*0.5f);
    }

    public bool IsDead => _isDead;
}