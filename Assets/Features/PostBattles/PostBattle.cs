using System;
using Features.Cards;
using Features.Maps.BattleNode;
using UnityEngine;

namespace Features.PostBattles
{
    public class PostBattle : MonoBehaviour
    {
        [SerializeField] private ExperienceObtained _experienceObtained;
        public event EventHandler Completed;
        public void Initialize(HeroRunCard heroRunCard, int expObtained)
        {
            _experienceObtained.Show(heroRunCard, expObtained);
            _experienceObtained.Completed += OnExpCompleted;
        }

        private void OnExpCompleted(object sender, EventArgs e)
        {
            _experienceObtained.Hide();
            Completed?.Invoke(this, null);
        }
    }
}