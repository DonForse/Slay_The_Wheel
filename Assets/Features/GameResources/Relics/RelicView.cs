using Features.Maps;
using UnityEngine;

namespace Features.GameResources.Relics
{
    public class RelicView : MonoBehaviour
    {
        public void Set(RelicScriptableObject relicScriptableObject)
        {
            RelicId = relicScriptableObject.id;
        }

        public RelicType RelicId { get; set; }
    }
}