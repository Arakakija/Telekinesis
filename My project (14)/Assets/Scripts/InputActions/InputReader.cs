using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerControls.IPlayerActions
{
    public event Action onJumpEvent;
    public event Action onDodgeEvent;
    public event Action onTargetEvent;
    public event Action onCancelTargetEvent;
    
    private PlayerControls _controls;

    public Vector2 MovementValue { get; private set; }
    
    
    public bool IsAttacking { get; private set; }
    public AttackType CurrentAttackType { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _controls = new PlayerControls();
        _controls.Player.SetCallbacks(this);
        
        _controls.Player.Enable();
    }

    private void OnDestroy()
    {
        _controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) {return;}
        onJumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(!context.performed) {return;}
        onDodgeEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        onTargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        onCancelTargetEvent?.Invoke();
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            IsAttacking = true;
            CurrentAttackType = AttackType.Light;
        }
        else if(context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            IsAttacking = true;
            CurrentAttackType = AttackType.Heavy;
        }
        else if(context.canceled)
        {
            IsAttacking = false;
        }
    }
}
