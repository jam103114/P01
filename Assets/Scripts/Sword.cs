using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement tpm;
   /* private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            tpm.hit = true;
            Debug.Log("Hit!!");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            tpm.hit = true;
            Debug.Log("Hit!!");
        }
    }
}
