using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Assignable
    public CharacterController controller;
    public Transform capsule;
    public Transform camera_;

    // Movement
    public float moveSpeed = 20f;
    private float x;
    private float z;

    // Ground Check
    public LayerMask groundMask;
    public Transform groundCheck;
    private bool isGrounded = true;
    private float groundDistance = 0.4f;

    // Crouch && Slide
    private Vector3 playerScale;
    private Vector3 crouchScale;
    private KeyCode crouch = KeyCode.LeftShift;
    public static bool isCrouching = false;
    public float crouchSpeed = 1.5f;
    public float slideSpeed = 5f;
    public float slideDuration = 1.2f;

    // Jump
    public float jumpForce = 20f;

    // Other
    private Vector3 velocity;
    public float gravity = -12f;
    public static bool sliding = false;
    private Vector3 cameraPos;
    public static bool cameraTrackingHead = false;

    void Start()
    {
        playerScale = capsule.transform.localScale;
        crouchScale = new Vector3(capsule.transform.localScale.x, capsule.transform.localScale.y - 0.5f, capsule.transform.localScale.z);
        cameraPos = camera_.transform.position;
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movement
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (!isCrouching)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }
        else if(isCrouching)
            controller.Move(move * moveSpeed * Time.deltaTime - (move * moveSpeed * Time.deltaTime) / crouchSpeed);


        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);

        // Crouch
        if (Input.GetKeyDown(crouch))
        {
            capsule.transform.localScale = crouchScale;
            controller.height = 0.88f;


            // Sliding
            //if (z > 0.85 && isGrounded)
            //{
            //    StartCoroutine(Slide());
            //}
            //else
                isCrouching = true;

        }
        if(Input.GetKeyUp(crouch))
        {
            isCrouching = false;
            capsule.transform.localScale = playerScale;
            controller.height = 2.06f;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /*
    IEnumerator Slide()
    {
        sliding = true;
        cameraTrackingHead = false;
        camera_.transform.position = new Vector3(camera_.position.x, transform.position.y, camera_.position.z);

        capsule.transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, 0.746f, transform.position.z);
        controller.height = 0.88f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * slideSpeed * slideDuration;

        float slideTimer = 0f;
        while (slideTimer < slideDuration)
        {
            camera_.transform.position = new Vector3(camera_.position.x, 1f, camera_.position.z);

            float t = slideTimer / slideDuration;
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);

            controller.Move(newPosition - transform.position);

            slideTimer += Time.deltaTime;
            yield return null;
        }

        controller.Move(endPosition - transform.position);
        Input.ResetInputAxes();

        camera_.transform.position = cameraPos;
        cameraTrackingHead = true;
        sliding = false;
    }
    */
}
