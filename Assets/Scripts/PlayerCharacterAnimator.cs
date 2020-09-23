using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;

    const string IdleState = "Idle";
    const string WalkState = "Walk";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string RunState = "Run";
    const string AttackState = "Attack";

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

    public void OnStartJumping()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    public void OnStartAttack()
    {
        _animator.Play("Attack");
    }

    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartWalking += OnStartWalking;
        _thirdPersonMovement.StartRunning += OnStartRunning;
        _thirdPersonMovement.StartJumping += OnStartJumping;
        _thirdPersonMovement.StartAttacking += OnStartAttack;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartWalking -= OnStartWalking;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
        _thirdPersonMovement.StartJumping -= OnStartJumping;
        _thirdPersonMovement.StartAttacking -= OnStartAttack;
    }
}
