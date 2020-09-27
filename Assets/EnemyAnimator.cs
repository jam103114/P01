using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    //[SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    //[SerializeField] Health _playerHealth = null;

    const string IdleState = "Idle";
    const string WalkState = "Walk";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string RunState = "Run";
    const string AttackState = "Attack";
    const string DamageState = "Damaged";
    const string DeathState = "Death";

    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        OnIdle();
    }

    public void OnIdle()
    {
        _animator.Play(IdleState);
    }

    private void OnStartRunning()
    {
        //TODO
    }

    public void OnStartWalking()
    {
        //TODO
    }

    public void OnStartJumping()
    {
        //TODO
    }

    public void OnStartAttack()
    {
        //TODO
    }

    public void OnTakeDamage()
    {
        //TODO    
    }

    public void OnDeath()
    {
        //TODO
    }

    private void OnEnable()
    {
        //_thirdPersonMovement.Idle += OnIdle;
    }

    private void OnDisable()
    {
        //_thirdPersonMovement.Idle += OnIdle;
    }
}
