using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public int size;
    public float radius;
    public float rotationSpeed;
    private float rotationAngle;
    private List<InPlayCard> inPlayCards = new ();
    public InPlayCard inPlayCardPrefab;
    private List<Vector2> positions = new List<Vector2>();

    private void OnEnable()
    {
        float angleOffset = Mathf.PI / 2; // Offset to start at the top of the circle

        for (var i = 0; i < size; i++)
        {
            // Calculate spherical coordinates with angle offset
            var theta = 2 * Mathf.PI * i / size + angleOffset;
            var x = radius * Mathf.Cos(theta);
            var y = radius * Mathf.Sin(theta);

            var position = new Vector2(x, y);
            positions.Add(position);

            // Create and position the object
            var go = Instantiate(inPlayCardPrefab, this.transform);
            go.transform.localPosition = position;
            inPlayCards.Add(go);
        }
    }
    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            //cancel or return.
            return;
        }
//if (current rotation > delta)
//stop rotating, execute action.
        RotateWithMouse();
    }

    private void RotateWithMouse()
    {
        var rotationInput = Input.GetAxis("Mouse X");
        rotationAngle += rotationInput * rotationSpeed * Time.deltaTime;

        // Apply rotation incrementally around the circle based on initial positions
        for (var i = 0; i < size; i++)
        {
            // Calculate the new angle for rotation (around the circle) based on initial angle and rotationAngle
            var initialTheta = Mathf.Atan2(positions[i].y, positions[i].x);
            var newTheta = initialTheta + rotationAngle;

            // Calculate new position using the updated angle
            var x = radius * Mathf.Cos(newTheta);
            var y = radius * Mathf.Sin(newTheta);

            var newPosition = new Vector2(x, y);
            inPlayCards[i].transform.localPosition = newPosition;
        }
    }
}