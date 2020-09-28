using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    int _currentHealth = 50;
    //[SerializeField] Animator _animator = null;
    //[SerializeField] GameObject _hpSlider = null;
    [SerializeField] Slider slider = null;
    [SerializeField] TextMeshProUGUI tmpHP = null;
    [SerializeField] TextMeshProUGUI tmpGameOver = null;
    [SerializeField] ThirdPersonMovement TPM = null;

    [SerializeField] AudioSource _aS = null;
    [SerializeField] AudioClip _hurt = null;

    public event Action death = delegate { };
    public event Action takeDamage = delegate { };

    private void Awake()
    {

        if (this.gameObject.tag == "Player")
        {
            TPM = gameObject.GetComponent<ThirdPersonMovement>();
            UpdateHealth();
            if(_aS == null)
            {
                _aS = this.gameObject.GetComponent<AudioSource>();
                _aS.clip = _hurt;
            }
        }
    }
    public void Heal(int amount)
    {
        _currentHealth += amount;
        Debug.Log(gameObject.name + " has healed " + amount);
        if(_currentHealth > 50)
        {
            _currentHealth = 50;
        }

        if(this.gameObject.tag == "Player")
        {
            UpdateHealth();
        }
    }

    public void TakeDamage(int amount)
    {
        if (this.gameObject.active)
        {
            _currentHealth -= amount;
            Debug.Log("Health.TakeDamage Was Called. Current Health: " + _currentHealth);
        }

        if(this.gameObject.tag == "Player")
        {
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                UpdateHealth();
                Kill();
            }

            if (_currentHealth >0)
            {
                takeDamage?.Invoke();
                _aS.clip = _hurt;
                _aS.Play();
                UpdateHealth();
            }
        }

        if(this.gameObject.tag == "Enemy")
        {
            if(_currentHealth <= 0)
            {
                this.gameObject.SetActive(false);
                //Destroy(this.gameObject);
            }
        }
    }

    public void Kill()
    {
        if (this.gameObject.tag == "Player")
        {
            if(TPM.alive == true)
            {
                death?.Invoke();
                Debug.Log("DEATH");
                slider.gameObject.SetActive(false);
                tmpHP.gameObject.SetActive(false);
                tmpGameOver.gameObject.SetActive(true);
            }
            TPM.alive = false;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void UpdateHealth()
    {
        slider.value = _currentHealth;
    }
}
