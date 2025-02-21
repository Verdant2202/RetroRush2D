using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    [SerializeField] private string runningAnimatorBoolName = "running";
    [SerializeField] private string walkingAnimatorBoolName = "walking";
    [SerializeField] private string jumpingAnimatorBoolName = "jumping";
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void HandleAnimation()
    {
        if (playerMovement.GetIsWalking())
        {
            animator.SetBool(walkingAnimatorBoolName, true);
        }
        else
        {
            animator.SetBool(walkingAnimatorBoolName, false);
        }

        if (playerMovement.GetIsRunning())
        {
            animator.SetBool(runningAnimatorBoolName, true);
        }
        else
        {
            animator.SetBool(runningAnimatorBoolName, false);
        }

        if (playerMovement.GetIsJumping())
        {
            animator.SetBool(jumpingAnimatorBoolName, true);
        }
        else
        {
            animator.SetBool(jumpingAnimatorBoolName, false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        HandleAnimation();
        if(playerMovement.GetMoveDirection().x >= 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }
}
