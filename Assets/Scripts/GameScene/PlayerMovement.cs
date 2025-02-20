using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveForce;
    private Vector2 gravityForce;
    [SerializeField] float gravity;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpForce = 6f;

    [SerializeField] BoxCollider2D groundCheckCollider;
    [SerializeField] LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void FixedUpdate()
    {
        moveForce = Vector2.zero;
        gravityForce = Vector2.down * gravity;

        if (Input.GetKey(KeyCode.A))
        {
            moveForce += Vector2.left * playerSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveForce += Vector2.right * playerSpeed;
        }
        Debug.Log(IsGrounded());
      

        rigidbody.velocity = new Vector2(moveForce.x, rigidbody.velocity.y);
        if (!IsGrounded())
        {
            rigidbody.velocity += gravityForce * Time.fixedDeltaTime;
        }

    }

    bool IsGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckCollider.transform.position, groundCheckCollider.size, 0, groundLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
