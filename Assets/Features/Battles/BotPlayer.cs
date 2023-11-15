using System;
using System.Collections;
using Features.Battles.Wheel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Battles
{
    public class BotPlayer : MonoBehaviour
    {
        [SerializeField] private WheelController wheelController;
        [SerializeField] private BotControlWheel botControlWheel;
        [SerializeField] private BusQueue busQueue;
        [SerializeField] private Battle battle;
        private bool turnEnabled;
        private float delay = 3f;
        private float timer = 3f;
        private void Update()
        {
            timer += Time.deltaTime;
            if (battle.Turn == Turn.Player) return;
            if (busQueue.HasPendingActions()) return;
            if (timer < delay) return;
            timer = 0f;
            if (battle.Actions > 0)
            {
                busQueue.EnqueueAction(TryExecuteBotAction());
            }

            if (wheelController.AllUnitsDead())
                this.enabled = false;
        }
        private IEnumerator TryExecuteBotAction()
        {
            yield return botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1);
        }
    }
}