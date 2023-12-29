using System;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using Features.Cards.Heroes;
using Unity.VisualScripting;
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
                var heroAbilities = _heroCardScriptableObject.abilities.ToList();
                var burn = heroAbilities.FirstOrDefault(x => x.Type == AbilityEnum.AddBurn);

                heroAbilities.Remove(burn);

                if (burn == null)
                    burn = new Ability()
                    {
                        Type = AbilityEnum.AddBurn,
                        AbilityData = new[]
                        {
                            new AbilityData()
                            {
                                Amount = 0,
                                Target =
                                    TargetEnum.AllEnemies
                            }
                        }
                    };
                else
                {
                    var burnAll = burn.AbilityData.FirstOrDefault(x => x.Target == TargetEnum.AllEnemies);
                    if (burnAll == null)
                    {
                        var newBurn = burn.AbilityData.ToList();
                        newBurn.Add(new AbilityData()
                        {
                            Amount = 0,
                            Target =
                                TargetEnum.AllEnemies
                        });
                        burn.AbilityData = newBurn.ToArray();
                    }

                }
                burn.AbilityData.First(x => x.Target == TargetEnum.AllEnemies).Amount++;
                heroAbilities.Add(burn);
                _heroCardScriptableObject.abilities = heroAbilities.ToList();
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