using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] AbilityLoadout _abilityLoadout;
    [SerializeField] Ability _startingAbility;
    [SerializeField] Ability _newAbilityToTest;
    [SerializeField] Transform _testTarget = null;
    [SerializeField] GameObject _playerObject = null;

    public Transform CurrentTarget { get; private set; }

    private void Awake()
    {
        if(_startingAbility != null)
        {
            _abilityLoadout?.EquipAbility(_startingAbility);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerObject.transform.LookAt(CurrentTarget);
            
            _abilityLoadout.UseEquippedAbility(CurrentTarget);

            
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            _abilityLoadout.EquipAbility(_newAbilityToTest);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SetTarget(_testTarget);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        CurrentTarget = newTarget;
        Debug.Log("Current Target " + newTarget);
    }
}
