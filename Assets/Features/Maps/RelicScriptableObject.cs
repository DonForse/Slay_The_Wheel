using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Maps
{
    [CreateAssetMenu(fileName = "Relic", menuName = "Relics/Relic")]
    public class RelicScriptableObject : ScriptableObject
    {
        public RelicSpectrumType Spectrum;
        public RelicType id;
        public string name;
    }
}