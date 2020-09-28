using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    [SerializeField] Health _playerHealth = null;

    const string IdleState = "Idle";
    const string WalkState = "Walk";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string RunState = "Run";
    const string AttackState = "Attack";
    const string DamageState = "Damaged";
    const string DeathState = "Death";
    const string CastFireball = "Fireball";
    const string CastSmite = "Smite";
    const string CastHeal = "Heal";

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
        _animator.Play(AttackState);
    }

    public void OnTakeDamage()
    {
        _animator.CrossFadeInFixedTime(DamageState, .2f);
    }

    public void OnDeath()
    {
        _animator.CrossFadeInFixedTime(DeathState, .2f);
    }

    public void OnCastFireball()
    {
        _animator.CrossFadeInFixedTime(CastFireball, .2f);
    }

    public void OnCastSmite()
    {
        _animator.CrossFadeInFixedTime(CastSmite, .2f);
    }

    public void OnCastHeal()
    {
        _animator.CrossFadeInFixedTime(CastHeal, .2f);
    }

    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartWalking += OnStartWalking;
        _thirdPersonMovement.StartRunning += OnStartRunning;
        _thirdPersonMovement.StartJumping += OnStartJumping;
        _thirdPersonMovement.StartAttacking += OnStartAttack;
        _playerHealth.takeDamage += OnTakeDamage;
        _playerHealth.death += OnDeath;
        _thirdPersonMovement.CastFireball += OnCastFireball;
        _thirdPersonMovement.CastSmite += OnCastSmite;
        _thirdPersonMovement.CastHeal += OnCastHeal;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartWalking -= OnStartWalking;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
        _thirdPersonMovement.StartJumping -= OnStartJumping;
        _thirdPersonMovement.StartAttacking -= OnStartAttack;
        _playerHealth.takeDamage -= OnTakeDamage;
        _playerHealth.death -= OnDeath;
        _thirdPersonMovement.CastFireball -= OnCastFireball;
        _thirdPersonMovement.CastSmite -= OnCastSmite;
        _thirdPersonMovement.CastHeal -= OnCastHeal;
    }
}
