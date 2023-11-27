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
        private HeroRunCardScriptableObject _heroCardScriptableObject;
        public event EventHandler<LevelUpUpgrade> Selected;
        public void Show(List<LevelUpUpgrade> infoLevelUpUpgrades, HeroRunCardScriptableObject heroRunCardScriptableObject)
        {
            canvas.SetActive(true);
            _heroCardScriptableObject = heroRunCardScriptableObject;
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
                var asd = _heroCardScriptableObject.onDealDamageAbilities.ToList();
                var burnAll=asd.FirstOrDefault(x => x.Type == AbilityEnum.BurnAll);
                asd.Remove(burnAll);
                if (burnAll == null)
                    burnAll = new Ability() { Type = AbilityEnum.BurnAll, Amount = 0 };
                burnAll.Amount++;
                asd.Add(burnAll);
                _heroCardScriptableObject.onDealDamageAbilities = asd.ToArray();
            }
            if (e == LevelUpUpgrade.Atk)
            {
                _heroCardScriptableObject.attack += 2;
            }
            if (e == LevelUpUpgrade.Hp)
            {
                _heroCardScriptableObject.hp += 20;
            }

            _heroCardScriptableObject.hp += Random.Range(0, 2) == 0 ? 5 : 10;
            _heroCardScriptableObject.attack += Random.Range(0,2);

            
            
            Selected?.Invoke(this, e);
        }

        public void Hide()
        {
            canvas.SetActive(false);
        }
    }
}