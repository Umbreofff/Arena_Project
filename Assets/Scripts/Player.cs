using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerControls playerControl;
    private CharacterController characterController;
    private Animator animator;
    private Transform playerTransform;
    private Vector3 currentMovement;
    private Vector3 cameraRelativeMovement;
    private bool isJumping;
    private bool isMoving;
    private float currentVelocity;
    private const float gravityScale = -9.81f;

    private int isJumpingHash;
    private int velocityHash;

    [SerializeField] private float jumpHeight = 100;
    [SerializeField] private float velocity = 10;

    private void Awake()
    {
        playerControl = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        GetAnimatorParameters();

        playerControl.PlayerActions.Move.started += OnMoveInput;
        playerControl.PlayerActions.Move.performed += OnMoveInput;
        playerControl.PlayerActions.Move.canceled += OnMoveInput;

        playerControl.PlayerActions.Jump.started += OnJumpInput;
        playerControl.PlayerActions.Jump.canceled += OnJumpInput;
    }

    void FixedUpdate()
    {
        MovePlayer();
        AnimationHandler();
        RotationHandler();
        GravityHandler();
    }

    private void GravityHandler()
    {
        currentMovement.y += gravityScale * Time.deltaTime;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 inputData = context.ReadValue<Vector2>();
        currentMovement.x = inputData.x;
        currentMovement.y = 0;
        currentMovement.z = inputData.y;
        isMoving = inputData.x != 0 || inputData.y != 0;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValueAsButton();
        Jump();
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y += Mathf.Sqrt(jumpHeight * gravityScale * -1);
        }
    }

    private void MovePlayer()
    {
        cameraRelativeMovement = ConverToCameraSpace(currentMovement);
        characterController.Move(cameraRelativeMovement * velocity * Time.deltaTime);
        currentVelocity = characterController.velocity.magnitude / 10f;
    }


    private void AnimationHandler()
    {
        bool isJumpingAnimation = animator.GetBool(isJumpingHash);
        animator.SetFloat(velocityHash, currentVelocity);

        if(isJumping && !isJumpingAnimation)
        {
            animator.SetBool(isJumpingHash, true);
        }
        else if(!isJumping && isJumpingAnimation)
        {
            animator.SetBool(isJumpingHash, false);
        }
    }

    private Vector3 ConverToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZproduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZproduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }


    private void RotationHandler()
    {
        float rotationFactorPerFrame = 10;
        Vector3 positionToLookAt;
        positionToLookAt.x = cameraRelativeMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = cameraRelativeMovement.z;
        Quaternion currentRotation = transform.rotation;

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void GetAnimatorParameters()
    {
        isJumpingHash = Animator.StringToHash("isJumping");
        velocityHash = Animator.StringToHash("velocity");
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }
}
