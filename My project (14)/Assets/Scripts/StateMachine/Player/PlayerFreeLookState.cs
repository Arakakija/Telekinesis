using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace StateMachine.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private static readonly int FreeLookSpeed = Animator.StringToHash("FreeLookSpeed");
        private static readonly int FreeLookBlendTree = Animator.StringToHash("FreeLookBlendTree");

        private const float AnimatorDampTime = 0.1f;
        private const float fixedTransitionDuration = 0.1f;
        
        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        

        public override void Enter()
        {
            _stateMachine.InputReader.onTargetEvent += OnTarget;
            _stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree,fixedTransitionDuration);
        }

        public override void Tick(float DeltaTime)
        {
            if (_stateMachine.InputReader.IsAttacking)
            {
                _stateMachine.SwitchState(new PlayerAttackingState(_stateMachine,_stateMachine.InputReader.CurrentAttackType,0));
                return;
            }
            
            Vector3 move = CalculateMovement();
            
            Move(move * _stateMachine.FreeLookMovementSpeed, DeltaTime);

            if (_stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                _stateMachine.Animator.SetFloat(FreeLookSpeed,0,AnimatorDampTime,DeltaTime);
                return;
            }
            
            _stateMachine.Animator.SetFloat(FreeLookSpeed,1,AnimatorDampTime,DeltaTime);
            FaceMovementDirection(move,DeltaTime);
        }

        public override void Exit()
        {
            _stateMachine.InputReader.onTargetEvent -= OnTarget;
        }

        private void OnTarget()
        {
            if (_stateMachine.Targeter.IsTargeting || !_stateMachine.Targeter.SelectTarget()) return;
            _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
        }

        private Vector3 CalculateMovement()
        {
            var cameraForward = _stateMachine.MainCameraTransform.forward;
            var cameraRight = _stateMachine.MainCameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            
            cameraForward.Normalize();
            cameraRight.Normalize();

            return cameraForward * _stateMachine.InputReader.MovementValue.y +
                   cameraRight * _stateMachine.InputReader.MovementValue.x;
        }

        private void FaceMovementDirection(Vector3 movement, float DeltaTime)
        {
            _stateMachine.transform.rotation = Quaternion.Lerp(_stateMachine.transform.rotation, Quaternion.LookRotation(movement),
                _stateMachine.RotationSmooth * DeltaTime);
        }


    }
}
