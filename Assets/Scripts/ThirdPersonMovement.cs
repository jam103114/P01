using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartWalking = delegate { };
    public event Action StartRunning = delegate { };

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float runSpeed = 12f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool _isMoving = false;
    bool _isRunning = false;

///////////////////////////////////////////////
    Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;


    private void Start()
    {
        Idle?.Invoke();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);




///////////////////////////////////////////////////////////////////////////////////////
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (direction.magnitude >= 0.1f)
            {
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
}
