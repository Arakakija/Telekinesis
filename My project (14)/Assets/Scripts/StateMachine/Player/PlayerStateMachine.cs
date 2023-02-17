using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public InputReader InputReader { get; private set;}
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Targeter Targeter { get; private set; }
        
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        
        
        public Transform MainCameraTransform { get; private set; }
        
        [field: SerializeField] public float FreeLookMovementSpeed { get; private set;}
        [field: SerializeField] public float TargetingMovementSpeed { get; private set;}
        [field: SerializeField] public float RotationSmooth { get; private set; }

        [field: SerializeField] public ComboArray DualBladeCombos { get; private set; }



        void Start()
        {
            if (Camera.main is not null) MainCameraTransform = Camera.main.transform;
            SwitchState(new PlayerFreeLookState(this));
        }


        
    }
}
