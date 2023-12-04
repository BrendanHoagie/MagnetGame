using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWallMover : MonoBehaviour
{
    public float moveSpeed = 2.0f;  // Adjust this value to control the speed of movement.
    public float maxYPosition = 10.0f; // Adjust this value to set the maximum Y position.
    public float minYPosition = 2.0f;  // Adjust this value to set the minimum Y position.
    private int direction = 1; // 1 for moving up, -1 for moving down.

    void Update()
    {
        // Move the wall up and down.
        transform.Translate(Vector3.up * moveSpeed * direction * Time.deltaTime);

        // Check if the wall has reached the maximum or minimum Y position.
        if (transform.position.y >= maxYPosition)
        {
            direction = -1; // Change direction to move down.
        }
        else if (transform.position.y <= minYPosition)
        {
            direction = 1; // Change direction to move up.
        }
    }
}