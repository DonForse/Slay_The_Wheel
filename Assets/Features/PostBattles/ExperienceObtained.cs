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
        private int _expRemaining;
        private HeroRunCardScriptableObject _hero;
        public event EventHandler Completed;

        public IEnumerator Show(HeroRunCardScriptableObject heroCardScriptableObject, int experienceObtained)
        {
            _hero = heroCardScriptableObject;
            _expRemaining = experienceObtained;
            
            continueButton.gameObject.SetActive(false);
            container.SetActive(true);
            yield return WaitSceneOpen();
            var info = levelUpsScriptableObject.LevelUpInformations[_hero.Level];
            
            progressBar.InitialFillValue = _hero.Exp / (float)info.ExpToLevel;
            
            if (_hero.Exp + experienceObtained >= info.ExpToLevel)
                yield return ShowLevelUp(info, experienceObtained);
            else
                yield return ShowExperienceRemainingObtained(experienceObtained, info);
        }

        private IEnumerator ShowExperienceRemainingObtained(int experienceObtained, LevelUpInformation info)
        {
            progressBar.UpdateBar01(Mathf.FloorToInt(_hero.Exp + experienceObtained) / (float)info.ExpToLevel);
            yield return WaitForProgressBarAnimation();
            _hero.Exp += experienceObtained;
            continueButton.gameObject.SetActive(true);
        }

        private IEnumerator ShowLevelUp(LevelUpInformation info, int experienceObtained)
        {
            if (_hero.Exp + experienceObtained < info.ExpToLevel) yield break;
            
            _expRemaining = experienceObtained - info.ExpToLevel - _hero.Exp;
            _hero.Level++;
            if (_hero.Level > levelUpsScriptableObject.LevelUpInformations.Count)
                _hero.Level = levelUpsScriptableObject.LevelUpInformations.Count;
            _hero.Exp = 0;
            info = levelUpsScriptableObject.LevelUpInformations[_hero.Level];
            
            progressBar.UpdateBar01(1);
            yield return WaitForProgressBarAnimation();


            selectUpgrade.Show(info.LevelUpUpgrades, _hero);
            selectUpgrade.Selected += OnUpgradeSelected;
            container.SetActive(false);
            progressBar.InitialFillValue = 0f;
            progressBar.UpdateBar01(0);
        }

        private void OnUpgradeSelected(object sender, LevelUpUpgrade e)
        {
            selectUpgrade.Hide();
            container.SetActive(true);
            StartCoroutine(Show(_hero, _expRemaining));
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


        private static WaitForSeconds WaitSceneOpen() => new(1.2f);
        private static WaitForSeconds WaitForProgressBarAnimation() => new(2f);
    }
}