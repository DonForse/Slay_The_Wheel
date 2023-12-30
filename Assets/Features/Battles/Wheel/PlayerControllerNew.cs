using System;
using Features.Battles.Core;
using Features.Battles.Spells;

namespace Features.Battles.Wheel
{
    public class PlayerControllerNew : IPlayerController
    {
        public event EventHandler<WheelRotation> SpinWheel;
        public event EventHandler<SpellView> ExecuteSpell;

        public event EventHandler SwapCard;
        public event EventHandler TurnEnd; 

        private WheelController _wheelController;
        public void Initialize()
        {
            _wheelController.RotatedLeft += OnSpinWheelLeft;
            _wheelController.RotatedRight += OnSpinWheelRight;
        }

        private void OnSpinWheelRight(object sender, EventArgs e)
        {
            SpinWheel?.Invoke(this, WheelRotation.Right);
        }
        private void OnSpinWheelLeft(object sender, EventArgs e)
        {
            SpinWheel?.Invoke(this, WheelRotation.Left);
        }
    }
}