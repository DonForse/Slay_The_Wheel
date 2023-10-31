using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Maps
{
    public class MapSpot : MonoBehaviour
    {
        public List<MapSpot> PossibleNextPositions;
        [SerializeField]private MapSpotType mapSpotType;
        [SerializeField]private Button button;
        [SerializeField] private Image spotImage;
        [SerializeField] private Sprite[] icons;
        private LineRenderer _lineRenderer;
        public EventHandler<MapSpotType> Selected;

        private void Start()
        {
            SetImage();
        }

        private void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.SetPositions(PossibleNextPositions.Select(x=>x.transform.position).ToArray());
        }

        private void SetImage()
        {
            spotImage.sprite = icons[(int)mapSpotType];
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            Selected?.Invoke(this,mapSpotType);
        }

        public void SetAvailable()
        {
            button.interactable = true;
            button.onClick.AddListener(OnButtonPressed);
        }
    }
}