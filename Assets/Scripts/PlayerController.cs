using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private float moveVector;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void Update() {

        HandlePlayerMovement();
    }

    void FixedUpdate() {
        transform.Translate(new Vector3(moveVector, 0, 0) * moveSpeed);
    }

    private void HandlePlayerMovement() {
        moveVector = Input.GetAxisRaw("Horizontal");
    }
}
