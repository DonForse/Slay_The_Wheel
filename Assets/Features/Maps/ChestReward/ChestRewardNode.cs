using System;
using System.Linq;
using Features.Common;
using Febucci.UI;
using UnityEngine;

namespace Features.Maps.ChestReward
{
    public class ChestRewardNode : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private Clickeable clickeableCanvas;
        [SerializeField] private GameObject relicsContainer;
        [SerializeField] private ChestRewardSelectionView chestRewardSelectionViewPrefab;
        [SerializeField] private RelicsScriptableObject relicsScriptableObject;
        [SerializeField] private int relicSelectionAmount;
        [SerializeField] private TypewriterByCharacter typewriterByCharacter;
        
        private bool _endDisplay;

        public event EventHandler<Relic> RelicSelected;

        private void OnEnable()
        {
            _endDisplay = false;
            typewriterByCharacter.onTextShowed.AddListener(TextCompleted);
            clickeableCanvas.Clicked += OnScreenSelected;
            //
            // yield return new WaitUntil(()=>_endDisplay);
        }

        private void OnDisable()
        {
            typewriterByCharacter.onTextShowed.RemoveListener(TextCompleted);
            clickeableCanvas.Clicked -= OnScreenSelected;
        }

        private void TextCompleted() => _endDisplay = true;
        private void OnScreenSelected(object sender, EventArgs eventArgs)=>typewriterByCharacter.SkipTypewriter();

        public void Show()
        {
            var playerRelics = Provider.PlayerPrefsRelicsRepository().Get();
            var goodRelics = relicsScriptableObject.relics.Where(relic =>
                relic.Spectrum == RelicSpectrumType.Good 
                && !playerRelics.Contains(relic.id)).ToList();
            
            foreach (Transform child in relicsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < relicSelectionAmount; i++)
            {
                var go  =Instantiate(chestRewardSelectionViewPrefab, relicsContainer.transform);
                go.Set(new Relic(goodRelics[i]));
                go.Selected+=OnRelicSelected;
            }

            canvas.SetActive(true);
            typewriterByCharacter.ShowText("<wave>WOW!!</wave> Is that a <rainb>CHEST?</rainb>");
        }

        public void Hide()
        {
            canvas.SetActive(false);
        }

        private void OnRelicSelected(object sender, Relic e)
        {
            RelicSelected?.Invoke(this, e);
        }
    }
}
