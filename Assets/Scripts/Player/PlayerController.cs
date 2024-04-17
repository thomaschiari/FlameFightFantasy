using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private int health = 3;

    private void Update()
    {
        // Get the input from the horizontal and vertical axes
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Create a movement vector and normalize it
        Vector2 movement = new Vector2(horizontal, vertical).normalized;

        // Apply the movement to the player's position
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            health--;
            Debug.Log("Player health: " + health);
        }
    }
}