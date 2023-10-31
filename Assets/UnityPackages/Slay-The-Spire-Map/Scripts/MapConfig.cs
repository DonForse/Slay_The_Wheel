using System.Collections.Generic;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.Scripts
{
    [CreateAssetMenu]
    public class MapConfig : ScriptableObject
    {
        public List<NodeBlueprint> nodeBlueprints;
        public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

        [OneLineWithHeader]
        public IntMinMax numOfPreBossNodes;
        [OneLineWithHeader]
        public IntMinMax numOfStartingNodes;

        [Tooltip("Increase this number to generate more paths")]
        public int extraPaths;
        [Reorderable]
        public ListOfMapLayers layers;

        [System.Serializable]
        public class ListOfMapLayers : ReorderableArray<MapLayer>
        {
        }
    }
}