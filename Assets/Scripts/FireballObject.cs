using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballObject : MonoBehaviour
{
    public int fbSpeed = 10;
    Rigidbody rb; 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = transform.forward * fbSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //DO A THING
        Debug.Log("Its a hit Captain");
       // if(collision.gameObject.tag != "Player")
        //{
            Destroy(this.gameObject);
        //}
    }
}
