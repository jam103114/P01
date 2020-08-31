using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;

    const string IdleState = "Idle";
    const string WalkState = "Walk";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string RunState = "Run";

    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    public void OnStartWalking()
    {
        _animator.CrossFadeInFixedTime(WalkState, .2f);
    }

    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartWalking += OnStartWalking;
        _thirdPersonMovement.StartRunning += OnStartRunning;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartWalking -= OnStartWalking;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
    }
}
