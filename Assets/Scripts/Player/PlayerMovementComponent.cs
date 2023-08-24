using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMovementComponent : MonoBehaviour
{
    private Vector3 currentMovement;
    private float playerVelocity;
    private CharacterController characterController;

    private bool isMoving;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerVelocity = PlayerManager.instance.GetPlayerVelocity();
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        Vector2 inputData = context.ReadValue<Vector2>();
        MoveHandler(inputData);
    }

    private void MoveHandler(Vector2 inputData)
    {
        currentMovement.x = inputData.x;
        currentMovement.y = 0;
        currentMovement.z = inputData.y;

        bool isMoving = inputData.x != 0 || inputData.y != 0; 
        PlayerManager.instance.SetIsMoving(isMoving);

        //currentVelocity = characterController.velocity.magnitude;
        characterController.Move(currentMovement * playerVelocity * Time.deltaTime);
        RotationHandler();
    }

    private void RotationHandler()
    {
        float rotationFactorPerFrame = 10;
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            //currentMovement.y += Mathf.Sqrt(jumpHeight * gravityScale * -1);
        }
    }
}
