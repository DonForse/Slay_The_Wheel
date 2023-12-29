using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Cards.HUD {
    /**
     * Offers a preview of a card when hovering over it by cloning the original card and placing it on top of the original.
     * Allows setting the global preview position and scale.
     */
    public class ClonePreviewManager : MonoBehaviour, CardPreviewManager {
        
        [SerializeField]
        private float verticalPosition;
        
        [SerializeField]
        private float previewScale = 1f;
        
        [SerializeField]
        private int previewSortingOrder = 1;
        
        private Dictionary<CardWrapper, Transform> previews = new();
        
        public void OnCardHover(object sender, CardWrapper card) {
            OnCardPreviewStarted(card);
        }
        
        public void OnCardUnhover(object sender, CardWrapper card) {
            OnCardPreviewEnded(card);
        }

        public void OnCardPreviewStarted(CardWrapper card) {
            if (!previews.ContainsKey(card)) {
                CreateCloneForCard(card);
            }

            var preview = previews[card];
            preview.gameObject.SetActive(true);
            preview.position = new Vector3(card.transform.position.x, verticalPosition, card.transform.position.z);
        }

        private void CreateCloneForCard(CardWrapper card) {
            var clone = Instantiate(card.gameObject, transform);
            clone.transform.position = card.transform.position;
            clone.transform.localScale = Vector3.one * previewScale;
            clone.transform.rotation = Quaternion.identity;
            var cloneCanvas = clone.GetComponent<Canvas>();
            cloneCanvas.sortingOrder = previewSortingOrder;
            StripCloneComponents(clone);
            previews.Add(card, clone.transform);
        }

        private static void StripCloneComponents(GameObject clone) {
            var cloneWrapper = clone.GetComponent<CardWrapper>();
            if (cloneWrapper != null) {
                Destroy(cloneWrapper);
            }

            var cloneRaycaster = clone.GetComponent<GraphicRaycaster>();
            if (cloneRaycaster != null) {
                Destroy(cloneRaycaster);
            }
        }

        public void OnCardPreviewEnded(CardWrapper card) {
            if (previews.ContainsKey(card))
                previews[card].gameObject.SetActive(false);
        }
    }
}