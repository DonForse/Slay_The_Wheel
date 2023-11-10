using UnityEngine;

namespace Features.Maps
{
    [CreateAssetMenu(fileName = "Relic", menuName = "Relics/Relic")]
    public class RelicScriptableObject : ScriptableObject
    {
        public RelicType id;
        public string name;
    }
}