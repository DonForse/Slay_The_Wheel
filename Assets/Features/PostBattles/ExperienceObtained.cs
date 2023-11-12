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

        public void Show(HeroRunCard heroCard, int experienceObtained)
        {
            continueButton.gameObject.SetActive(false);
            container.SetActive(true);
            var info = levelUpsScriptableObject.LevelUpInformations[heroCard.Level];
            progressBar.InitialFillValue = heroCard.Exp / (float)info.ExpToLevel;
            if (heroCard.Exp + experienceObtained >= info.ExpToLevel)
            {
                progressBar.UpdateBar01(1);
                StartCoroutine(ShowLevelUp(info, heroCard, experienceObtained));
            }
            else
            {
                progressBar.UpdateBar01(Mathf.FloorToInt(heroCard.Exp + experienceObtained) / (float)info.ExpToLevel);
                heroCard.Exp += experienceObtained;
                continueButton.gameObject.SetActive(true);
            }
            // levelUpsScriptableObject.LevelUpInformations.Firs
            // if (heroCard.Exp + experienceObtained)
        }

        private IEnumerator ShowLevelUp(LevelUpInformation info, HeroRunCard heroCard, int experienceObtained)
        {
            var expRemaining = info.ExpToLevel - (heroCard.Exp + experienceObtained);
            heroCard.Level++;
            if (heroCard.Level > levelUpsScriptableObject.LevelUpInformations.Count)
                heroCard.Level = levelUpsScriptableObject.LevelUpInformations.Count;
            heroCard.Exp = 0;
            heroCard.Exp += expRemaining;
            info = levelUpsScriptableObject.LevelUpInformations[heroCard.Level];
            progressBar.UpdateBar01(Mathf.FloorToInt(expRemaining / (float)info.ExpToLevel));
            container.SetActive(false);
            selectUpgrade.Show(info.LevelUpUpgrades, heroCard);
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