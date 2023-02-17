using System.Collections;
using System.Collections.Generic;
using StateMachine.Player;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private AttackAnimData _currentAttack;

    private bool _forceWasApplied;
    public PlayerAttackingState(PlayerStateMachine stateMachine, AttackType attackType, int attackIndex) : base(stateMachine)
    {
        _currentAttack = attackType switch
        {
            AttackType.Light => stateMachine.DualBladeCombos.lightCombo[attackIndex],
            AttackType.Heavy => stateMachine.DualBladeCombos.heavyCombo[attackIndex],
            _ => _currentAttack
        };
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(_currentAttack.AnimationName,_currentAttack.TransitionDuration);
    }

    public override void Tick(float DeltaTime)
    {
        Move(DeltaTime);
        FaceTarget();
        float normalizeTime = GetNormalizeTime();
        
        if (normalizeTime >= previousFrameTime && normalizeTime < 1.0f)
        {
            if (normalizeTime >= _currentAttack.ForceTime)
            {
                TryApplyForce();
            }
            

            if (_stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizeTime);
            }
        }
        else
        {
            if (_stateMachine.Targeter.CurrentTarget != null)
            {
                _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            }
            else
            {
                _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
            }
        }
        previousFrameTime = normalizeTime;
    }

    public override void Exit()
    {
    }

    private float GetNormalizeTime()
    {
        var currentAnimatorStateInfo = _stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        var nextAnimatorStateInfo = _stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (_stateMachine.Animator.IsInTransition(0) && nextAnimatorStateInfo.IsTag("Attack"))
        {
            return nextAnimatorStateInfo.normalizedTime;
        }
        else if (!_stateMachine.Animator.IsInTransition(0) && currentAnimatorStateInfo.IsTag("Attack"))
        {
            return currentAnimatorStateInfo.normalizedTime;
        }

        return 0f;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (_currentAttack.ComboStateIndex == -1) return;
        
        if(normalizedTime < _currentAttack.ComboAttackTime) return;
        
        _stateMachine.SwitchState(
            new PlayerAttackingState(
                _stateMachine,
                _stateMachine.InputReader.CurrentAttackType,
                _currentAttack.ComboStateIndex)
            );
    }

    private void TryApplyForce()
    {
        if(_forceWasApplied) return;
        _stateMachine.ForceReceiver.AddForce(_stateMachine.transform.forward * _currentAttack.Force);
        _forceWasApplied = true;
    }
}
