using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext, float> HandleMoveInput;
    private Transform playerTransform;
    private bool isJumping;
    private bool isMoving;

    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float velocity = 10;
    [SerializeField] private int lives = 1;

    private void Awake()
    {        
        PlayerManagerSetUpListeners();
    }

    private void PlayerManagerSetUpListeners()
    {
        GameSystem.OnMoveInputContextReceived += MovePlayer;
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        isMoving = context.ReadValue<Vector2>().x != 0 || context.ReadValue<Vector2>().y != 0;
        HandleMoveInput?.Invoke(context, velocity);
    }

    private void OnDisable()
    {
        GameSystem.OnMoveInputContextReceived -= MovePlayer;
    }

}
