using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public static Playercontroller instance;

    public CharacterController characterController;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followcamera;

    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundPlayer;
    [SerializeField] private float jumpHeight = 2.5f;

    public Animator animator;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (checkwinner.instance.isWinner)
        {
            case true:
                animator.SetBool("victory", checkwinner.instance.isWinner);
                break;

            case false:
            Movement();
            break;
        }
    }

    void Movement()
    {

        groundPlayer = characterController.isGrounded;
        if (characterController.isGrounded && playerVelocity.y < -2f)
        {
            playerVelocity.y = -1f;
        }


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        Vector3 movementInput = Quaternion.Euler(0,followcamera.transform.eulerAngles.y,0)
                *new Vector3 (horizontalInput,0,verticalInput);

        Vector3 movementDirection = movementInput.normalized;


        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);

        if(movementDirection != Vector3.zero) 
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,desiredRotation,rotationSpeed * Time.deltaTime);
        }


        if(Input.GetButtonDown("Jump") && groundPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
            animator.SetTrigger("Jumping Up");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

       animator.SetFloat("Speed",Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", characterController.isGrounded);
    }
}
