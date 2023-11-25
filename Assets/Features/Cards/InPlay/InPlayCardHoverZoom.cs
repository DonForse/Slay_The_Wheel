using UnityEngine;

namespace Features.Cards.InPlay
{
    public class InPlayCardHoverZoom : MonoBehaviour
    {
        [SerializeField]private float activeScale;
        [SerializeField]private float inactiveScale;
        
        public bool IsActive;

        private bool _dragging;
        private Vector3 _previousScale;
        private Vector3 _activeScale;
        private Vector3 _inactiveScale;

        private void OnEnable()
        {
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            _activeScale = Vector3.one * activeScale;
            _inactiveScale = Vector3.one * inactiveScale;
        }

        private void OnMouseDown()
        {
            _previousScale = this.transform.localScale;
            _dragging = true;
        }
        
        private void OnMouseUp()
        {
            _dragging = false;
            this.transform.localScale = _previousScale;
        }

        private void OnMouseOver()
        {
            _previousScale = this.transform.localScale; 
            this.transform.localScale =_activeScale;
        }

        private void OnMouseExit()
        {
            if (_dragging) return;
            
            this.transform.localScale = IsActive ? _activeScale : _inactiveScale;
        }
    }
}