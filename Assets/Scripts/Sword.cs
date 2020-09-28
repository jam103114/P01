using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement tpm;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.active)
            {
                tpm.hit = true;
                tpm.hitTarget = other;
                Debug.Log("Hit!!");
            }
            
            if(other.gameObject.active == false)
            {
                tpm.hit = false;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            tpm.hit = false;
        }
    }
}
