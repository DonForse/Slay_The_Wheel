using UnityEngine;

namespace Features.Cards.InPlay
{
    public class InPlayCardHoverZoom : MonoBehaviour
    {
        [SerializeField]private float activeScale;
        [SerializeField]private float inactiveScale;
        
        // public bool IsActive;

        private bool _dragging;
        private Vector3 _previousScale;
        private Vector3 _activeScale;
        private Vector3 _inactiveScale;

        public void SetScale(float range)
        {
            var clampedRange = Mathf.Clamp01(range);
            var value = Mathf.Lerp(inactiveScale, activeScale, clampedRange);
            this.transform.localScale = Vector3.one * value;
        }

        private void OnEnable()
        {
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            _activeScale = Vector3.one * activeScale;
            _inactiveScale = Vector3.one * inactiveScale;
        }

        private void OnMouseEnter()
        {
            _previousScale = this.transform.localScale; 
            this.transform.localScale =_activeScale;
        }

        private void OnMouseExit()
        {
            if (_dragging) return;
            this.transform.localScale =_previousScale;

            // this.transform.localScale = IsActive ? _activeScale : _inactiveScale;
        }

        public void SetActive(bool value)
        {
            // IsActive = value;
            // this.transform.localScale = IsActive ? _activeScale : _inactiveScale;
        }
    }
}