using System.Linq;
using UnityEngine;

namespace Features.Maps
{
    [CreateAssetMenu(fileName = "Relics", menuName = "Relics/Relics")]
    public class RelicsScriptableObject : ScriptableObject
    {
        public RelicScriptableObject[] relics;

        public RelicScriptableObject Find(RelicType relic)
        {
            return relics.FirstOrDefault(x => x.id == relic);
        }
    }
}