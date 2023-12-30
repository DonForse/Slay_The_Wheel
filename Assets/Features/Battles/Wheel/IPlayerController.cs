using System;
using System.Collections.Generic;
using Features.Battles.Core;
using Features.Battles.Spells;

namespace Features.Battles.Wheel
{
    public interface IPlayerController
    {
        event EventHandler<WheelRotation> SpinWheel;
        event EventHandler<SpellView> ExecuteSpell;
        event EventHandler SwapCard;
        event EventHandler TurnEnd; 
    }

    public class BattlePlayer
    {
        private IPlayerController _playerController;
        public List<WheelSlot> Slots = new();
        
        public void Initialize()
        {
            _playerController.SpinWheel += OnSpinWheel;
        }

        private void OnSpinWheel(object sender, WheelRotation e)
        {
            throw new NotImplementedException();
        }
    }
}