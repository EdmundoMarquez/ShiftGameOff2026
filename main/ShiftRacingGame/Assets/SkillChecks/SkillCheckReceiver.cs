using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillCheckReceiver : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions _inputActions;
    public event Action ActionPressed;
    
    private void OnEnable()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.AddCallbacks(this);
        _inputActions.Player.Enable();
    }
    
    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
    
    public void OnMove(InputAction.CallbackContext context) { }

    public void OnAction(InputAction.CallbackContext context)
    {
        if(context.performed)
            ActionPressed?.Invoke();
    }
}
