using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] AbilityLoadout _abilityLoadout;
    [SerializeField] Ability _heal;
    [SerializeField] Ability _fireball;
    [SerializeField] Ability _smite;
    [SerializeField] Transform _testTarget = null;
    [SerializeField] Transform _testTarget2 = null;
    [SerializeField] GameObject _playerObject = null;
    [SerializeField] AudioSource _audioSource = null;
    [SerializeField] AudioClip _clipSmite;
    [SerializeField] AudioClip _clipAttack;
    [SerializeField] AudioClip _clipFireball;
    [SerializeField] AudioClip _clipAttack2;
    [SerializeField] AudioClip _clipHeal;
    [SerializeField] GameObject _projectileSpawned = null;
    [SerializeField] TMP_Text _btnFireball;
    [SerializeField] TMP_Text _btnHeal;
    [SerializeField] TMP_Text _btnSmite;

    public Transform CurrentTarget { get; private set; }


    public event Action Idle = delegate { };
    public event Action StartWalking = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartJumping = delegate { };
    public event Action StartAttacking = delegate { };
    public event Action CastFireball = delegate { };
    public event Action CastSmite = delegate { };
    public event Action CastHeal = delegate { };

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float runSpeed = 12f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool _isMoving = false;
    bool _isRunning = false;
    public bool hit = false;

    public Animator _anim;

///////////////////////////////////////////////
    Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;
    public bool alive = true;
    Transform target = null;
    public Collider hitTarget = null;
    public float fireballTimer = 10f;
    public float smiteTimer = 10f;
    public int healCounter = 3;
    public bool fbTimer = false;
    public bool smtTimer = false;


    ///////
    public int tarVal = 0;
    Enemy[] targets = null;

    private void Awake()
    {
        if (_heal != null)
        {
            _abilityLoadout?.EquipAbility(_heal);
        }
    }
    private void Start()
    {
        _anim.GetComponent<Animator>();
        Idle?.Invoke();
        targets = GameObject.FindObjectsOfType<Enemy>();
        if(targets == null)
        {
            Debug.Log("No game objects are tagged with 'Enemy'");
        }
        target = targets[tarVal].gameObject.transform.GetChild(2);
        target.gameObject.SetActive(true);
        SetTarget(targets[tarVal].transform);
        _btnHeal.text = healCounter.ToString();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(alive == true)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                _anim.StopPlayback();
                StartJumping?.Invoke();
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(smtTimer == true)
        {
            smiteTimer -= Time.deltaTime;

            if(smiteTimer <= 0)
            {
                smiteTimer = 0;
                smtTimer = false;
                _btnSmite.gameObject.SetActive(false);
            }
            _btnSmite.text = smiteTimer.ToString("F0");
        }

        if (fbTimer == true)
        {
            fireballTimer -= Time.deltaTime;

            if (fireballTimer <= 0)
            {
                fireballTimer = 0;
                fbTimer = false;
                _btnFireball.gameObject.SetActive(false);
            }
            _btnFireball.text = fireballTimer.ToString("F0");
        }

        if (alive == true)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (direction.magnitude >= 0.1f)
                {
                    _anim.StopPlayback();
                    CheckIfStartRunning();
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);
                }
                else
                {
                    CheckIfStoppedMoving();
                }
            }
            else
            {
                if (direction.magnitude >= 0.1f)
                {
                    _anim.StopPlayback();
                    CheckIfStartedMoving();
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);
                }
                else
                {
                    CheckIfStoppedMoving();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(fbTimer == false)
                {
                    CastFireball?.Invoke();
                    _abilityLoadout.EquipAbility(_fireball);
                    _playerObject.transform.LookAt(CurrentTarget);
                    _abilityLoadout.UseEquippedAbility(CurrentTarget);
                    _audioSource.clip = _clipFireball;
                    _audioSource.Play();
                    fbTimer = true;
                    fireballTimer = 5f;
                    _btnFireball.gameObject.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if(smtTimer == false)
                {
                    CastSmite?.Invoke();
                    _abilityLoadout.EquipAbility(_smite);
                    _abilityLoadout.UseEquippedAbility(CurrentTarget);
                    _audioSource.clip = _clipSmite;
                    _audioSource.Play();
                    Health health = CurrentTarget.gameObject.GetComponent<Health>();
                    health.TakeDamage(10);
                    smtTimer = true;
                    smiteTimer = 10f;
                    _btnSmite.gameObject.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if(healCounter > 0)
                {
                    CastHeal?.Invoke();
                    _abilityLoadout.EquipAbility(_heal);
                    _audioSource.clip = _clipHeal;
                    _audioSource.Play();
                    _abilityLoadout.UseEquippedAbility(_playerObject.transform);
                    healCounter--;
                    _btnHeal.text = healCounter.ToString();
                }
                else
                {
                    Debug.Log("You are out of heals");
                }

            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TargetSystem();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartAttacking?.Invoke();
                _audioSource.clip = _clipAttack2;
                _audioSource.Play();
                if (hit == true)
                {
                    GameObject projectile = Instantiate(_projectileSpawned, hitTarget.gameObject.transform.position, hitTarget.gameObject.transform.rotation);
                    _audioSource.clip = _clipAttack;
                    _audioSource.Play();
                    Health enemyHP = hitTarget.gameObject.GetComponent<Health>();
                    enemyHP.TakeDamage(10);
                    if (hitTarget.gameObject.active == false)
                    {
                        hit = false;
                    }
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        if (targets[tarVal].gameObject.active == false)
        {
            target.gameObject.SetActive(false);
            if (targets.Length - 1 == tarVal)
            {
                tarVal = 0;
                Debug.Log("Target equals " + targets.Length);
            }
            else
            {
                tarVal++;
                Debug.Log("Target else " + tarVal);
            }
            SetTarget(targets[tarVal].transform);
            target = targets[tarVal].gameObject.transform.GetChild(2);
            target.gameObject.SetActive(true);
        }
    }


    private void CheckIfStartRunning()
    {
        
        _isMoving = true;
        if (_isRunning == false)
        {
            StartRunning?.Invoke();
        }
        _isRunning = false;
        
    }
    private void CheckIfStartedMoving()
    {
        _isRunning = true;
        if (_isMoving == false) 
        {
            StartWalking?.Invoke();
            Debug.Log("Started");
        }
        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isMoving == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }
        _isMoving = false;
    }

    public void SetTarget(Transform newTarget)
    {
        CurrentTarget = newTarget;
        Debug.Log("Current Target " + newTarget);
    }

    public void TargetSystem()
    {
        target.gameObject.SetActive(false);
        if (targets.Length - 1 == tarVal)
        {
            tarVal = 0;
            Debug.Log("Target equals " + targets.Length);
        }
        else
        {
            tarVal++;
            Debug.Log("Target else " + tarVal);
        }
        SetTarget(targets[tarVal].transform);
        target = targets[tarVal].gameObject.transform.GetChild(2);
        target.gameObject.SetActive(true);
    }

}
