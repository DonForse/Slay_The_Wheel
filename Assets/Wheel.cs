using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public int size;
    public float radius;
    public float rotationSpeed;
    private float rotationAngle;
    private List<InPlayCard> inPlayCards = new List<InPlayCard>();
    public InPlayCard inPlayCardPrefab;
    private List<Vector2> positions = new List<Vector2>();
    private bool isRotating = false;
    private float startAngle;

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
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            sr.color = new Color(i / 1f, i / 1f, i / 1f, 1f);
            go.transform.localPosition = position;
            inPlayCards.Add(go);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            startAngle = rotationAngle;

        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
            SnapToNearestPosition();
        }

        if (!isRotating) return;
        
        var rotationInput = Input.GetAxis("Mouse X");
        rotationAngle += rotationInput * rotationSpeed * Time.deltaTime;

        var anglePerItem = (1.5f * Mathf.PI) / (size);

        if (Mathf.Abs(rotationAngle - startAngle) >= anglePerItem)
        {
            isRotating = false;
            SnapToNearestPosition();
        }
        
        RotateWithMouse();
    }

    private void SnapToNearestPosition()
    {
        float anglePerItem = 2 * Mathf.PI / size;
        float targetAngle = Mathf.Round(rotationAngle / anglePerItem) * anglePerItem;
        rotationAngle = targetAngle;
        RotateWithMouse();
    }

    private void RotateWithMouse()
    {
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
