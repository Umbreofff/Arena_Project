using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControl;

    public event Action<InputAction.CallbackContext> OnMove;

    public void Awake()
    {
         playerControl = new PlayerControls();

         playerControl.PlayerActions.Move.started += OnMoveInput;
         playerControl.PlayerActions.Move.performed += OnMoveInput;
         playerControl.PlayerActions.Move.canceled += OnMoveInput;

         playerControl.PlayerActions.Jump.started += OnJumpInput;
         playerControl.PlayerActions.Jump.canceled += OnJumpInput;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        //isJumping = context.ReadValueAsButton();
        //Jump();
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
