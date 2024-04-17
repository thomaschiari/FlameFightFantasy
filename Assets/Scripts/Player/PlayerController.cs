using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float collisionOffset = 0.1f;
    public ContactFilter2D movementFilter;

    Vector2 moveInput;
    private int health = 3;
    private Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); // List to store all the collisions


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            bool success = TryMove(moveInput); // Try to move in any direction of input (could be diagonal)
            if (!success)
            {
                TryMove(new Vector2(moveInput.x, 0)); // Try to move horizontally
                if (!success)
                {
                    TryMove(new Vector2(0, moveInput.y)); // Try to move vertically
                }
            }

        }
    }

    private bool TryMove(Vector2 direction){
        int count = rb.Cast( // Cast a box in the direction we want to move
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>();
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