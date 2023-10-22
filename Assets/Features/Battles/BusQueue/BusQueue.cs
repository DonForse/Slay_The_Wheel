using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusQueue : MonoBehaviour
{
    private Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();
    private bool isProcessingActions = false;

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

        while (actionQueue.Count > 0)
        {
            var action = actionQueue.Dequeue();
            yield return action;

            // Wait for the current action to complete (you can customize this)
            yield return new WaitForSeconds(0.05f);
        }

        isProcessingActions = false;
    }

    public void Clear() => actionQueue.Clear();
}