using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.Example
{
    public class GameObjectExample : MonoBehaviour
    {

        //There's a bug with Unity and rendering when an Object has no CustomEditor defined. As in this example
        //The list will reorder correctly, but depth sorting and animation will not update :(
        [Reorderable(paginate = true, pageSize = 2)]
        public GameObjectList list;

        [System.Serializable]
        public class GameObjectList : ReorderableArray<GameObject>
        {
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                list.Add(gameObject);
            }
        }
    }
}