using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsView : MonoBehaviour
{
    [SerializeField] private GameObject iconActionPrefab;
    [SerializeField] private Transform container;

    public void ShowRemaining(int amount)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < amount; i++)
        {
            Instantiate(iconActionPrefab, container);
        }
    }
}