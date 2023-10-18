using System;
using System.Collections.Generic;
using Features.Cards;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Maps
{
    public class Shop : MonoBehaviour
    {
        [SerializeField][Range(0,8)] private int amountOfCards;
        [SerializeField] private GameObject container;
        [SerializeField] private ShopCard shopCardPrefab;
        public event EventHandler<BaseCardScriptableObject> CardSelected; 
        public void Show(List<BaseCardScriptableObject> baseCardScriptableObjects)
        {
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < amountOfCards; i++)
            {
                var go  =Instantiate(shopCardPrefab, container.transform);
                go.Set(baseCardScriptableObjects[Random.Range(0, baseCardScriptableObjects.Count)]);
                go.OnClick += OnCardSelected;
            }

            container.SetActive(true);
        }

        private void OnCardSelected(object sender, BaseCardScriptableObject e)
        {
            CardSelected?.Invoke(this, e);
        }
    }
}