using System.Collections;
using System.Collections.Generic;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class PlayerTargetingState : PlayerBaseState
{
    private static readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
    private static readonly int TargetingForward = Animator.StringToHash("TargetingForwardSpeed");
    private static readonly int TargetingRight = Animator.StringToHash("TargetingRightSpeed");
    
    private const float fixedTransitionDuration = 0.1f;
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.onTargetEvent += OnTarget;
    
        _stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree,fixedTransitionDuration);
    }

    public override void Tick(float DeltaTime)
    {
        if (_stateMachine.InputReader.IsAttacking)
        {
            _stateMachine.SwitchState(new PlayerAttackingState(_stateMachine,_stateMachine.InputReader.CurrentAttackType,0));
            return;
        }
        
        if (_stateMachine.Targeter.CurrentTarget != null)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * _stateMachine.TargetingMovementSpeed, DeltaTime);
            UpdateAnimator(DeltaTime);
            FaceTarget();
            return;
        }
        _stateMachine.Targeter.CancelTarget();
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }

    public override void Exit()
    {
        _stateMachine.InputReader.onTargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        if (!_stateMachine.Targeter.IsTargeting) return;
        _stateMachine.Targeter.CancelTarget();
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        var transform = _stateMachine.transform;
        movement += transform.right * _stateMachine.InputReader.MovementValue.x;
        movement += transform.forward * _stateMachine.InputReader.MovementValue.y;
        
        return movement;
    }

    private void UpdateAnimator(float DeltaTime)
    {
        if (_stateMachine.InputReader.MovementValue.x == 0)
        {
            _stateMachine.Animator.SetFloat(TargetingRight, 0, 0.1f, DeltaTime);
        }
        else
        {
            float value = _stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            _stateMachine.Animator.SetFloat(TargetingRight, value, 0.1f, DeltaTime);
        }


        if (_stateMachine.InputReader.MovementValue.y == 0)
        {
            _stateMachine.Animator.SetFloat(TargetingForward, 0, 0.1f, DeltaTime);
        }
        else
        {
            float value = _stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            _stateMachine.Animator.SetFloat(TargetingForward, value, 0.1f, DeltaTime);
        }
    }
    
}
