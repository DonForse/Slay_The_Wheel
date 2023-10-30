using System.Collections.Generic;
using UnityEngine;

namespace Features.Maps
{
    public class DebugMap : MonoBehaviour
    {
        [SerializeField] private Map map;
        // Start is called before the first frame update
        void Start()
        {
            map.Initialize();

        }
    }
}
