using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
    public Collider smiteTarget = null;


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

///////////////////////////////////////////////////////////////////////////////////////
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(alive == true)
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
                CastFireball?.Invoke();
                _abilityLoadout.EquipAbility(_fireball);
                _playerObject.transform.LookAt(CurrentTarget);
                _abilityLoadout.UseEquippedAbility(CurrentTarget);
                _audioSource.clip = _clipFireball;
                _audioSource.Play();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Needs to be smite
                //SetTarget(smiteTarget.gameObject.transform.GetChild(0).gameObject.transform);
                CastSmite?.Invoke();
                _abilityLoadout.EquipAbility(_smite);
                _abilityLoadout.UseEquippedAbility(CurrentTarget);
                _audioSource.clip = _clipSmite;
                _audioSource.Play();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CastHeal?.Invoke();
                _abilityLoadout.EquipAbility(_heal);
                _audioSource.clip = _clipSmite;
                _audioSource.Play();
                _abilityLoadout.UseEquippedAbility(_playerObject.transform);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                target.gameObject.SetActive(false);
                if(targets.Length - 1 == tarVal)
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

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartAttacking?.Invoke();
                _audioSource.clip = _clipAttack;
                _audioSource.Play();
                if (hit == true)
                {
                    //SetTarget(smiteTarget.gameObject.transform.GetChild(0).gameObject.transform);
                    _abilityLoadout.EquipAbility(_smite);
                    _abilityLoadout.UseEquippedAbility(CurrentTarget);
                    _audioSource.clip = _clipSmite;
                    _audioSource.Play();
                }
                hit = false;
                Debug.Log("Smite target " + smiteTarget.gameObject.transform.GetChild(0));

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

}
