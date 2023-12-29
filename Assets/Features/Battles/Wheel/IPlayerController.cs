using System;
using System.Collections.Generic;
using Features.Battles.Core;
using Features.Battles.Spells;
using Features.Cards.InPlay;

namespace Features.Battles.Wheel
{
    public interface IPlayerController
    {
        event EventHandler<WheelRotation> SpinWheel;
        event EventHandler<SpellView> ExecuteSpell;
        event EventHandler SwapCard;
    }

    public class BattlePlayer
    {
        private IPlayerController _playerController;
        public List<InPlayCardSlot> Slots = new();
        
        private IControlWheel input;
        private ControlWheel[] wheelControllers;
        private WheelController _wheelMovement;
        
        public void Initialize()
        {
            _playerController.SpinWheel += OnSpinWheel;
        }

        private void OnSpinWheel(object sender, WheelRotation e)
        {
            throw new NotImplementedException();
        }
    }

    public class InPlayCardSlot
    {
        public InPlayCard Card;
        public int index;
    }
}