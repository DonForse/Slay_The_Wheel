using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Common
{
    public class CoroutineManager : MonoBehaviour
    {
        public IEnumerator ExecuteCoroutines(params IEnumerator[] coroutines)
        {
            bool[] coroutineCompleted = new bool[coroutines.Length];

            for (int i = 0; i < coroutines.Length; i++)
            {
                StartCoroutine(RunCoroutineWithCallback(coroutines[i], i, coroutineCompleted));
            }

            // Wait for all coroutines to complete
            yield return new WaitUntil(() => coroutineCompleted.All(x => x));
            
            Debug.Log("All coroutines have finished.");
        }
        
        private IEnumerator RunCoroutineWithCallback(IEnumerator coroutine, int index, bool[] coroutineCompleted)
        {
            yield return StartCoroutine(coroutine);
            coroutineCompleted[index] = true;
        }
    }
}