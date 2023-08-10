using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedIndicator : MonoBehaviour
{
    public bool isGrounded;
    public PlayerMovement pMove;
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Scenario"))
        {
            isGrounded = false;
        }
    }
    private void OnTriggerStay(Collider other) 
    {
        if (other.CompareTag("Scenario"))
        {
            isGrounded = true;
            pMove.canDash = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Scenario"))
        {
            pMove.canJump = true;
            pMove.canDoubleJump = false;
            pMove.doubleJumping = false;
            pMove.jumping = false;
            pMove.canDash = true;
            pMove.anim.SetTrigger("Idle");
            pMove.DashTrail.emitting = false;
        }
    }
}
