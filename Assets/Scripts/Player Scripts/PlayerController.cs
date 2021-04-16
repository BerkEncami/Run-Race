using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 move;
    public float speed, jumpForce, gravity, verctialVelocity;
    

    private bool wallSlide, turn, superJump;

    private CharacterController charController;
    private Animator anim;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        gameObject.name = PlayerPrefs.GetString("PlayerName", "Player");
    }

    void Update()
    {
        if (GameManager.Instance.finish)
        {
            move = Vector3.zero;
            if (!charController.isGrounded)
                verctialVelocity -= gravity * Time.deltaTime;
            else
                verctialVelocity = 0;

            move.y = verctialVelocity;

            charController.Move(new Vector3(0, move.y * Time.deltaTime, 0));
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
            {
                anim.SetTrigger("Dance");
                transform.eulerAngles = Vector3.up * 180;
                
            }
            return;
        }
        if (!GameManager.Instance.start)
            return;

        move = Vector3.zero;
        move = transform.forward;

        if (charController.isGrounded)
        {
            wallSlide = false;
            verctialVelocity = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }

            if (turn)
            {
                turn = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180,
                    transform.eulerAngles.z);
            }
        }

        if (superJump)
        {
            superJump = false;
            verctialVelocity = jumpForce * 1.75f;

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                anim.SetTrigger("Jump");
        }

        if (!wallSlide)
        {
            gravity = 30;
            verctialVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            gravity = 15;
            verctialVelocity -= gravity * Time.deltaTime;

        }

        anim.SetBool("WallSlide", wallSlide);
        anim.SetBool("Grounded", charController.isGrounded);

        move.Normalize();
        move *= speed;
        move.y = verctialVelocity;
        charController.Move(move * Time.deltaTime);
    }

    void Jump()
    {
        verctialVelocity = jumpForce;
        anim.SetTrigger("Jump");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!charController.isGrounded)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Slide")
            {
                if (verctialVelocity < -.6f)
                    wallSlide = true;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();

                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180,
                        transform.eulerAngles.z);

                    wallSlide = false;
                }
            }
        }
        else
        {
            if (hit.collider.tag == "Trampoline")
                superJump = true;

            if (transform.forward != hit.collider.transform.up && transform.forward != hit.transform.right
                && hit.collider.tag == "Ground" && !turn)
                turn = true;

        }
    }
}
