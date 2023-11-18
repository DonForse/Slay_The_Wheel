using System;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using Features.Cards.Heroes;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Features.PostBattles
{
    public class SelectLevelUpUpgrade : MonoBehaviour
    {
        [SerializeField]private GameObject canvas;
        [SerializeField]private Transform container;
        [SerializeField]private LevelUpUpgradeView levelUpPrefab;
        private HeroRunCard _heroCard;
        public event EventHandler<LevelUpUpgrade> Selected;
        public void Show(List<LevelUpUpgrade> infoLevelUpUpgrades, HeroRunCard heroRunCard)
        {
            canvas.SetActive(true);
            _heroCard = heroRunCard;
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var levelUpUpgrade in infoLevelUpUpgrades)
            {
                var go  =Instantiate(levelUpPrefab, container);
                go.Set(levelUpUpgrade);
                go.Selected += OnSelected;
            }
        }

        private void OnSelected(object sender, LevelUpUpgrade e)
        {
            if (e == LevelUpUpgrade.Burn)
            {
                var asd = _heroCard.OnDealDamageAbilities.ToList();
                var burnAll=asd.FirstOrDefault(x => x.Type == AbilityEnum.BurnAll);
                asd.Remove(burnAll);
                if (burnAll == null)
                    burnAll = new Ability() { Type = AbilityEnum.BurnAll, Amount = 0 };
                burnAll.Amount++;
                asd.Add(burnAll);
                _heroCard.OnDealDamageAbilities = asd.ToArray();
            }
            if (e == LevelUpUpgrade.Atk)
            {
                _heroCard.Attack += 2;
            }
            if (e == LevelUpUpgrade.Hp)
            {
                _heroCard.Hp += 20;
            }

            _heroCard.Hp += Random.Range(0, 2) == 0 ? 5 : 10;
            _heroCard.Attack += Random.Range(0,2);

            
            
            Selected?.Invoke(this, e);
        }

        public void Hide()
        {
            canvas.SetActive(false);
        }
    }
}