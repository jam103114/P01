using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : Ability
{
    [SerializeField] GameObject _projectileSpawned = null;
    int _healAmount = 25;

    public override void Use(Transform origin, Transform target)
    {
        if(target == null) { return; }
        Debug.Log("Cast Cure." + target.gameObject.name);
        target.GetComponent<Health>()?.Heal(_healAmount);

        GameObject projectile = Instantiate(_projectileSpawned, origin.position, origin.rotation);
        if (target != null)
        {
            projectile.transform.LookAt(target);
        }
        Destroy(projectile, 3.5f);
    }
}
