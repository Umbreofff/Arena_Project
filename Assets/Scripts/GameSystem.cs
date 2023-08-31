using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private InputManager inputManager;

    public static Action<InputAction.CallbackContext> OnMoveInputContextReceived;

    private void Awake()
    {
        inputManager.OnMove += OnMoveInputReceived;
    }

    private void OnMoveInputReceived(InputAction.CallbackContext context)
    {
        OnMoveInputContextReceived(context);
    }

    private void OnDisable()
    {
        inputManager.OnMove -= OnMoveInputReceived;
    }
}
