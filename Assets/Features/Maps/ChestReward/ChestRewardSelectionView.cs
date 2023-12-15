using System;
using Febucci.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Features.Maps.ChestReward
{
    public class ChestRewardSelectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text relicName;
        private Relic _relic;
        public event EventHandler<Relic> Selected;

      


        public void Set(Relic relic)
        {
            _relic = relic;
            relicName.text = relic.RelicBase.name;
        }

        [UsedImplicitly]
        public void OnSelected()
        {
            Selected?.Invoke(this, _relic);
        }

    }
}