using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smite : Ability
{
    [SerializeField] GameObject _smiteSpawned = null;
    int _rank = 1;

    public override void Use(Transform origin, Transform target)
    {
        GameObject projectile = Instantiate(_smiteSpawned, target.position, target.rotation);
        if (target != null)
        {
            projectile.transform.LookAt(target);
        }
        Destroy(projectile, 3.5f);
        Debug.Log("Cast a rank " + _rank + " smite on " + target.gameObject.name + " !");
    }
}
