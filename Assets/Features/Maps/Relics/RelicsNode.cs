using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Maps.Relics
{
    public class RelicsNode : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject relicsContainer;
        [SerializeField] private RelicSelectionView relicSelectionViewPrefab;
        [SerializeField] private int relicSelectionAmount;
        public event EventHandler<Relic> RelicSelected;

        public void Show(List<RelicScriptableObject> relics)
        {
            foreach (Transform child in relicsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < relicSelectionAmount; i++)
            {
                var go  =Instantiate(relicSelectionViewPrefab, relicsContainer.transform);
                go.Set(new Relic(relics[i]));
                go.Selected+=OnRelicSelected;
            }

            canvas.SetActive(true);
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
