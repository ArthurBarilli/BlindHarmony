using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("components")]
    public CharacterController controller;
    public GroundedIndicator gController;

    [Header("Atributes")]
    public float speed;
    public float jumpHeight;
    public float doubleJumpHeight;
    public float groundedTimer;
    public float timeToGround;
    private Vector3 heighVelocity;
    public float gravity = -9.87f;
    private float baseGravity = -9.87f;
    public Vector3 dashDir;
    public float dashSpeed;
    public float dashDuration;
    public Animator anim;

    [Header("Bools")]
    public bool jumping;
    public bool doubleJumping;
    public bool canJump;
    public bool isGrounded;
    public bool canDoubleJump;
    public bool canDash;
    public bool isDashing;

    [Header("VFX")]
    public ParticleSystem DashEffect;
    public ParticleSystem doubleJumpEffect;
    public TrailRenderer DashTrail;

    [Header("Sounds")]
    public AudioClip dashSound;
    public AudioClip jumpSound;
    public AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //move axis
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        Vector3 dir = new( Horizontal, 0, Vertical);

        if (dir != Vector3.zero)
        {
            gameObject.transform.forward = dir;
        }

        // height speed
        if (gController.isGrounded && heighVelocity.y < 0)
        {
            heighVelocity.y = 0f;
        }
        if (!isDashing && dir != Vector3.zero)
        {
            controller.Move(dir * speed * Time.deltaTime);
            anim.SetBool("Run",true);
        }
        else
        {
            anim.SetBool("Run",false);
        }
        if (dir != new Vector3(0,0,0))
        {
            dashDir = dir.normalized;
        }
        

        //jump command
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump == true)
        {
            heighVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            gravity = baseGravity;
            DashTrail.emitting = true;
            jumping = true;
            canJump = false;
            audio.clip = jumpSound;
            audio.Play();
        }
        //double jump
        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump == true && canJump == false)
        {
            heighVelocity.y += Mathf.Sqrt(doubleJumpHeight * -3.0f * gravity);
            gravity = baseGravity;
            anim.SetTrigger("DoubleJump");
            doubleJumpEffect.Play();
            canDoubleJump = false;
            doubleJumping = true;
            audio.clip = jumpSound;
            audio.Play();
        }
        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {
            isDashing = true;
            canDash = false;
            DashEffect.Play();
            audio.clip = dashSound;
            audio.Play();
            StartCoroutine(Dash());
        }

        heighVelocity.y += gravity * Time.deltaTime;
        controller.Move(heighVelocity * Time.deltaTime);


        //jumping variables
        if(gController.isGrounded)
        {
            isGrounded = true;
            groundedTimer = 0;
            gravity = baseGravity;
        }
        else if(!gController.isGrounded && groundedTimer > timeToGround)
        {
            isGrounded = false;
            if(doubleJumping == false)
            {
                canDoubleJump = true;
            }
        }
        else if(!gController.isGrounded)
        {
            groundedTimer += Time.deltaTime;
        }
        if (!isGrounded)
        {
            gravity -= Time.deltaTime * 10;
        }
        //dashing
        if(isDashing)
        {
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
            
        }

        if(jumping)
        {
            anim.SetBool("Jump",true);
        }
        else
        {
            anim.SetBool("Jump",false);
        }

    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
}
