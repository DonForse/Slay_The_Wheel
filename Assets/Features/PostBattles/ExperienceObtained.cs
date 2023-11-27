using System;
using System.Collections;
using Features.Cards;
using Features.Cards.Heroes;
using JetBrains.Annotations;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PostBattles
{
    public class ExperienceObtained : MonoBehaviour
    {
        [SerializeField] private MMProgressBar progressBar;
        [SerializeField] private GameObject container;
        [SerializeField] private LevelUpsScriptableObject levelUpsScriptableObject;
        [SerializeField] private Button continueButton;
        [SerializeField] private SelectLevelUpUpgrade selectUpgrade;
        public event EventHandler Completed;

        public void Show(HeroRunCardScriptableObject heroCardScriptableObject, int experienceObtained)
        {
            continueButton.gameObject.SetActive(false);
            container.SetActive(true);
            var info = levelUpsScriptableObject.LevelUpInformations[heroCardScriptableObject.Level];
            progressBar.InitialFillValue = heroCardScriptableObject.Exp / (float)info.ExpToLevel;
            if (heroCardScriptableObject.Exp + experienceObtained >= info.ExpToLevel)
            {
                progressBar.UpdateBar01(1);
                StartCoroutine(ShowLevelUp(info, heroCardScriptableObject, experienceObtained));
            }
            else
            {
                progressBar.UpdateBar01(Mathf.FloorToInt(heroCardScriptableObject.Exp + experienceObtained) / (float)info.ExpToLevel);
                heroCardScriptableObject.Exp += experienceObtained;
                continueButton.gameObject.SetActive(true);
            }
            // levelUpsScriptableObject.LevelUpInformations.Firs
            // if (heroCard.Exp + experienceObtained)
        }

        private IEnumerator ShowLevelUp(LevelUpInformation info, HeroRunCardScriptableObject heroCardScriptableObject, int experienceObtained)
        {
            var expRemaining = info.ExpToLevel - (heroCardScriptableObject.Exp + experienceObtained);
            heroCardScriptableObject.Level++;
            if (heroCardScriptableObject.Level > levelUpsScriptableObject.LevelUpInformations.Count)
                heroCardScriptableObject.Level = levelUpsScriptableObject.LevelUpInformations.Count;
            heroCardScriptableObject.Exp = 0;
            heroCardScriptableObject.Exp += expRemaining;
            info = levelUpsScriptableObject.LevelUpInformations[heroCardScriptableObject.Level];
            progressBar.UpdateBar01(Mathf.FloorToInt(expRemaining / (float)info.ExpToLevel));
            container.SetActive(false);
            selectUpgrade.Show(info.LevelUpUpgrades, heroCardScriptableObject);
            selectUpgrade.Selected += OnUpgradeSelected;
            
            yield break;
        }

        private void OnUpgradeSelected(object sender, LevelUpUpgrade e)
        {
            container.SetActive(true);
            selectUpgrade.Hide();
            continueButton.gameObject.SetActive(true);
        }

        [UsedImplicitly]
        public void Continue()
        {
            Completed?.Invoke(this, null);
        }

        public void Hide()
        {
            container.SetActive(false);
        }
    }
}