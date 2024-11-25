using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed;
    public new Rigidbody2D rigidbody2D;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // Player 2's custom input keys (WASD for example)
        moveInput.x =
            Input.GetKey(KeyCode.J) ? -1
            : Input.GetKey(KeyCode.L) ? 1
            : 0;
        moveInput.y =
            Input.GetKey(KeyCode.I) ? 1
            : Input.GetKey(KeyCode.K) ? -1
            : 0;

        moveInput.Normalize();

        rigidbody2D.velocity = moveInput * moveSpeed;
    }
}
