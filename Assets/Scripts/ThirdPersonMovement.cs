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


    private void Start()
    {
        Idle?.Invoke();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //float fire3 = Input.GetAxis("Fire3") > 0;


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
