using Features.Cards;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Maps.Shop
{
    public class CardReveal : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private MMF_Player turnFeedback;
        // Start is called before the first rame update

        private void OnDisable()
        {
            button.onClick.RemoveListener(PlayFeedback);
        }

        public void Set(BaseCardScriptableObject card)
        {
            image.sprite = card.cardSprite;
            button.onClick.AddListener(PlayFeedback);
        }

        private void PlayFeedback()
        {
            turnFeedback.PlayFeedbacks();
        }
    }
}
