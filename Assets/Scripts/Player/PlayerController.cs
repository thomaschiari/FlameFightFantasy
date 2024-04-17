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
    Vector2 lastMoveDirection = Vector2.right; // Default facing direction
    private int health = 3;
    private Rigidbody2D rb;
    private Animator animator; // Reference to the Animator component

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            bool success = TryMove(moveInput);
            if (!success)
            {
                success = TryMove(new Vector2(moveInput.x, 0));
                if (!success)
                {
                    success = TryMove(new Vector2(0, moveInput.y));
                }
            }
            animator.SetFloat("Horizontal", moveInput.x); // Update the Horizontal parameter
            animator.SetFloat("Vertical", moveInput.y); // Update the Vertical parameter
            UpdateFacingDirection(moveInput); // Update the facing direction
            animator.SetBool("IsMoving", true); // Set isMoving to true when the player is moving
        }
        else
        {
            animator.SetFloat("Horizontal", 0); // Reset the Horizontal parameter
            animator.SetFloat("Vertical", 0); // Reset the Vertical parameter
            animator.SetBool("IsMoving", false); // Set isMoving to false when the player is not moving
        }
    }

    private void UpdateFacingDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            lastMoveDirection = direction; // Save the last move direction
            animator.SetFloat("FacingHorizontal", lastMoveDirection.x);
            animator.SetFloat("FacingVertical", lastMoveDirection.y);
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
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
