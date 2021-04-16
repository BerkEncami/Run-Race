using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Vector3 move;
    public float speed, jumpForce, gravity, verctialVelocity;

    private bool wallSlide, jump, superJump;

    private CharacterController charController;
    private Animator anim;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        gameObject.name = Names.BotNames[Random.Range(0, Names.BotNames.Length)];
    }

    void Update()
    {
        if (GameManager.Instance.finish)
        {
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
            jump = true;
            verctialVelocity = 0;
            RayCasting();
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

        anim.SetBool("Grounded", charController.isGrounded);
        anim.SetBool("WallSlide", wallSlide);

        move.Normalize();
        move *= speed;
        move.y = verctialVelocity;
        charController.Move(move * Time.deltaTime);
    }

    void RayCasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 8f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.tag == "Wall")
            {
                verctialVelocity = jumpForce;
                anim.SetTrigger("Jump");
            }
        }
    }

    IEnumerator LateJump(float time)
    {
        jump = false;
        wallSlide = true;
        yield return new WaitForSeconds(time);

        if (!charController.isGrounded)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180,
                    transform.eulerAngles.z);
            verctialVelocity = jumpForce;
            anim.SetTrigger("Jump");
        }
        jump = true;
        wallSlide = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Wall")
        {
            if (jump)
                StartCoroutine(LateJump(Random.Range(0.2f, .5f)));

            if (verctialVelocity < 0)
                wallSlide = true;
        }

        if (hit.collider.tag == "Trampoline" && charController.isGrounded)
            superJump = true;

        if (hit.collider.tag == "Slide" && charController.isGrounded)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180,
                    transform.eulerAngles.z);
        }
        else if (hit.collider.tag == "Slide")
        {
            wallSlide = true;
        }
    }
}
