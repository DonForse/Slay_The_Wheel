using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Battles.BusQueue
{
    public class BusQueue : MonoBehaviour
    {
        private Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();
        private bool isProcessingActions = false;
        private int TestCount;

        public void EnqueueAction(IEnumerator action)
        {
            actionQueue.Enqueue(action);

            // If no actions are currently being processed, start processing them.
            if (!isProcessingActions)
            {
                StartCoroutine(ProcessActions());
            }
        }
    
        public void EnqueueInterruptAction(IEnumerator interruptAction)
        {
            // Insert the interrupt action at the beginning of the queue.
            var tempQueue = new Queue<IEnumerator>();
            TestCount = actionQueue.Count;
            tempQueue.Enqueue(interruptAction);

            while (actionQueue.Count > 0)
            {
                tempQueue.Enqueue(actionQueue.Dequeue());
            }

            actionQueue = tempQueue;

            // If no actions are currently being processed, start processing them.
            if (!isProcessingActions)
            {
                StartCoroutine(ProcessActions());
            }
        }

        private IEnumerator ProcessActions()
        {
            isProcessingActions = true;

            TestCount = actionQueue.Count;
            while (actionQueue.Count > 0)
            {
                var action = actionQueue.Dequeue();
                TestCount = actionQueue.Count;
                yield return action;

                // Wait for the current action to complete (you can customize this)
                yield return new WaitForSeconds(0.05f);
            }

            isProcessingActions = false;
        }

        public void Clear() => actionQueue.Clear();

        public bool HasPendingActions() => actionQueue.Count > 0;
    }
}