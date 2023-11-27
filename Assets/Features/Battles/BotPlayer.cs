using System;
using System.Collections;
using Features.Battles.Core;
using Features.Battles.Wheel;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Features.Battles
{
    public class BotPlayer : MonoBehaviour
    {
        [FormerlySerializedAs("wheelController")] [SerializeField] private PlayerController playerController;
        [SerializeField] private BotControlWheel botControlWheel;
        [SerializeField] private BusQueue.BusQueue busQueue;
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

            if (playerController.AllUnitsDead())
                this.enabled = false;
        }
        private IEnumerator TryExecuteBotAction()
        {
            yield return botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1);
        }
    }
}