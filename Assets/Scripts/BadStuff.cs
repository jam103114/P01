using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadStuff : MonoBehaviour
{
    public int _damage = 5;
    public float timer = 5f;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Health health = other.GetComponent<Health>();
            health?.TakeDamage(_damage);
            Debug.Log("Don't Stand here! This hurts! You took " + _damage);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Health health = other.GetComponent<Health>();
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            health?.TakeDamage(_damage);
            timer = 5f;
            Debug.Log("This still hurts! You took " + _damage);
        }
    }
}
