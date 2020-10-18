using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// CharacterController를 이용하여 플레이어를 움직이고 싶다.
public class PlayerMove : MonoBehaviour
{
    float speed;
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;

    CharacterController cc;
    public float gravity = -20;
    float yVelocity; // 수직속도
    public float jumpPower = 3f;
    int jumpCount;
    int maxJumpCount = 1;

    public float crouchSpeed = 2.2f;
    public float crouchHeight;
    public bool isCrouching;
    private float playerHeight;

    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    // public Animator anim;
    // [SerializeField] private AudioClip jumpSound;
    // private AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerHeight = 2;
        isWalking = true;
        isCrouching = false;
        // anim = GetComponent<Animator>();
        // audioSource = GetComponent<AudioSource>();
    }
    // Shift를 누르면서 조작을 하게 되면 스피드를 두 1.5배로 올리고 싶다.
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        // 카메라가 바라보는 방향으로 이동하고 싶다.
        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

        if (isGrounded())
        {
            yVelocity = 0;
            jumpCount = 0;
            // anim.SetBool("IsJumping", false);
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            {
                speed = sprintSpeed;
                isWalking = false;
                // anim.SetFloat("Speed", 1f);
            }

            else if (Input.GetKey(KeyCode.C))
            {
                cc.height = crouchHeight;
                isCrouching = true;
                speed = crouchSpeed;
            }

            else if (Input.GetKeyUp(KeyCode.C))
            {
                cc.height = 2;
                isCrouching = false;
                speed = walkSpeed;
            }

            else
            {
                speed = walkSpeed;
                isWalking = true;
                // anim.SetFloat("Speed", 0f);
            }

            dir *= speed;

        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            jumpCount++;
            isJumping = true;

            yVelocity = jumpPower;
        }


        dir.y = yVelocity;
        cc.Move(dir * Time.deltaTime);

    }

    private bool isGrounded()
    {
        if (cc.isGrounded)
        {
            isJumping = false;
            return true;
        }

        Vector3 bottom = cc.transform.position - new Vector3(0, cc.height / 2, 0);

        RaycastHit hit;
        if (Physics.Raycast(bottom, new Vector3(0, -1, 0), out hit, 0.20f) && !isJumping)
        {
            cc.Move(new Vector3(0, -hit.distance, 0));
            return true;
        }
        else return false;
    }

    //IEnumerator Crouch()
    //{
    //    cc.height = crouchHeight;
    //    yield return new WaitForSeconds(0.5f);
    //    isCrouching = true;
    //    print("앉다");
    //}

    //IEnumerator StandUp()
    //{
    //    cc.height = 2;
    //    yield return new WaitForSeconds(0.5f);
    //    isCrouching = false;
    //    print("일어서다");
    //}
}
