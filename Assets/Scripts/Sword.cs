﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement tpm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            tpm.hit = true;
            tpm.smiteTarget = other;
            Debug.Log("Hit!!");
        }
    }
}
