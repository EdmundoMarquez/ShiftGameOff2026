using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Vehicle : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private SkillCheckIndicator skillCheckIndicator;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float changeSpeedBonus = 5f;
    private InputSystem_Actions _inputActions;
    private Rigidbody _selfRigidbody;
    private Vector3 _moveDirection;
    private float _currentSpeed;

    private void Start()
    {
        _selfRigidbody = GetComponent<Rigidbody>();
        skillCheckIndicator.TriggerCheckResult += OnTriggerCheckResult;
    }

    private void OnTriggerCheckResult(float result)
    {
        //Result ranges between 0 and 1, values close to limits are considered for decreasing speed
        float impulseForce = 0f;
        if(result <= 0.2f)
        {
            _currentSpeed -= changeSpeedBonus;
            impulseForce = -_currentSpeed;
        }
        else if(result <= 0.6f)
        {
            _currentSpeed += changeSpeedBonus * 0.5f;
            impulseForce = _currentSpeed * 0.5f;
        }
        else if(result <= 0.8f)
        {
            _currentSpeed += changeSpeedBonus;
            impulseForce = _currentSpeed;
        }
        else
        {
            _currentSpeed -= changeSpeedBonus;
            impulseForce = -_currentSpeed;
        }

        _selfRigidbody.AddForce(Vector3.forward * impulseForce, ForceMode.Impulse);
        _currentSpeed = Mathf.Clamp(_currentSpeed, baseSpeed, 100f);
        Debug.Log("Changed speed to: " + _currentSpeed);
    }

    private void FixedUpdate()
    {
        float horizontal = _moveDirection.x * Time.deltaTime * baseSpeed;
        transform.Translate(horizontal, 0, Time.deltaTime * baseSpeed);
    }

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
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _moveDirection.x  = moveInput.x;
    }

    public void OnAction(InputAction.CallbackContext context) { }
}
