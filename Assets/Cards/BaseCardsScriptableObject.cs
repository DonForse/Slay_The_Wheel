using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseCards", menuName = "Cards/Base Card List")]
public class BaseCardsScriptableObject : ScriptableObject
{
    public List<BaseCardScriptableObject> cards = new();
}