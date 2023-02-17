using UnityEngine;

namespace StateMachine.Player
{
    public abstract class PlayerBaseState : IState
    {
        protected PlayerStateMachine _stateMachine;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._stateMachine = stateMachine;
        }
        
        public abstract void Enter();
        public abstract void Tick(float DeltaTime);
        public abstract void Exit();

        protected void Move(Vector3 motion, float DeltaTime)
        {
            _stateMachine.CharacterController.Move((motion + _stateMachine.ForceReceiver.Movement) * DeltaTime);
        }

        protected void Move(float DeltaTime)
        {
            _stateMachine.CharacterController.Move(_stateMachine.ForceReceiver.Movement * DeltaTime);
        }

        protected void FaceTarget()
        {
            if(_stateMachine.Targeter.CurrentTarget == null) return;
            Vector3 distanceToTarget = _stateMachine.Targeter.CurrentTarget.transform.position -
                                       _stateMachine.transform.position;

            distanceToTarget.y = 0;

            _stateMachine.transform.rotation = Quaternion.LookRotation(distanceToTarget);
        }


    }
}
