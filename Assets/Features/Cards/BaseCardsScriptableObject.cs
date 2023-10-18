using System.Collections.Generic;
using UnityEngine;

namespace Features.Cards
{
    [CreateAssetMenu(fileName = "BaseCards", menuName = "Cards/Base Card List")]
    public class BaseCardsScriptableObject : ScriptableObject
    {
        public List<BaseCardScriptableObject> cards = new();
    }
}