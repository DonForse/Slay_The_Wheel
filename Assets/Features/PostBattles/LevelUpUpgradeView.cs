using System;
using Features.Cards.Heroes;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PostBattles
{
    public class LevelUpUpgradeView : MonoBehaviour
    {
        [SerializeField] private Button selectedButton;
        [SerializeField] private TMP_Text levelUpName;
        private LevelUpUpgrade _levelUpUpgrade;
        public event EventHandler<LevelUpUpgrade> Selected;

        public void Set(LevelUpUpgrade levelUpUpgrade)
        {
            _levelUpUpgrade = levelUpUpgrade;
            levelUpName.text = levelUpUpgrade.ToString();
            selectedButton.onClick.AddListener(ButtonSelected);
        }
        
        public void ButtonSelected()
        {
            Selected?.Invoke(this, _levelUpUpgrade);
        }
    }
}