using System.Linq;
using UnityEngine;

namespace Features.Maps
{
    public class MapLine : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void SetLine(Transform[] transforms)
        {
            _lineRenderer.SetPositions(transforms.Select(x => x.position).ToArray());
        }
    }
}
