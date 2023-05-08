using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundMask;
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private float moveVector;
    public bool isGrounded = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void Update() {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, groundCheckDistance, groundMask)) {
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }
        HandlePlayerMovement();
    }

    void FixedUpdate() {
        transform.Translate(new Vector3(moveVector, 0, 0) * moveSpeed);
    }

    private void HandlePlayerMovement() {
        moveVector = Input.GetAxisRaw("Horizontal");
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }
}
