using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveForce;
    private Vector2 gravityForce;
    [Header("General Movement Variables")]
    [SerializeField] private float gravity;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerSpeedRunning;
    [SerializeField] private float jumpForce = 6f;
    private float currentPlayerSpeed;
    [Header("Ground Variables")]
    [SerializeField] private BoxCollider2D groundCheckCollider;
    [SerializeField] private LayerMask groundLayerMask;


    [SerializeField] PlayerVisual playerVisual;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isJumping = false;

    [SerializeField] private float jumpTime = 2f;
    private float timeAtUnjump = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayerSpeed = playerSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if(Time.time >= timeAtUnjump)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            isJumping = true;
            timeAtUnjump = Time.time + jumpTime;
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (isWalking && Input.GetKey(KeyCode.LeftShift))
        {
            currentPlayerSpeed = playerSpeedRunning;
            isRunning = true;
        }
        else
        {
            currentPlayerSpeed = playerSpeed;
            isRunning = false;
        }

    }
    void FixedUpdate()
    {
        moveForce = Vector2.zero;
        gravityForce = Vector2.down * gravity;

        if (Input.GetKey(KeyCode.A))
        {
            moveForce += Vector2.left * currentPlayerSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveForce += Vector2.right * currentPlayerSpeed;
        }
      

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

    public bool GetIsRunning()
    {
        return isRunning;
    }

    public bool GetIsWalking()
    {
        return isWalking;
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }

    public Vector2 GetMoveDirection()
    {
        return moveForce;
    }

}
