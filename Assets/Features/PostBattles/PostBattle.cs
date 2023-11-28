using System;
using Features.Cards;
using UnityEngine;

namespace Features.PostBattles
{
    public class PostBattle : MonoBehaviour
    {
        [SerializeField] private ExperienceObtained _experienceObtained;
        public event EventHandler Completed;
        public void Initialize(HeroRunCardScriptableObject heroRunCardScriptableObject, int expObtained)
        {
            StartCoroutine(_experienceObtained.Show(heroRunCardScriptableObject, expObtained));
            _experienceObtained.Completed += OnExpCompleted;
        }

        private void OnExpCompleted(object sender, EventArgs e)
        {
            _experienceObtained.Hide();
            Completed?.Invoke(this, null);
        }
    }
}