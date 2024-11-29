using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float maxHealth = 100f;
    [Space]
    public float hunger;
    public float maxHunger = 100f;
    [Space]
    public float thirst;
    public float maxThirst = 100f;

    [Header("Stats Depletion")]
    public float hungerDepletion = 0.5f;
    public float thirstDepletion = 0.75f;


    [Header("Stats Damages")]
    public float hungerDamage = 1.5f;
    public float thirstDamage = 2.25f;

    [Header("UI")]
    public StatsBar healthBar;
    public StatsBar hungerBar;
    public StatsBar thirstBar;


    private void Start()
    {
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
    }

    private void Update()
    {
        UpdateState();
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthBar.numberText.text = health.ToString("f0");
        healthBar.bar.fillAmount = health / 100;

        hungerBar.numberText.text = hunger.ToString("f0");
        hungerBar.bar.fillAmount = hunger / 100;

        thirstBar.numberText.text = thirst.ToString("f0");
        thirstBar.bar.fillAmount = thirst / 100;
    }

    public void UpdateState()
    {
        if (health <= 0)
            health = 0;
        if (health >= maxHealth)
            health = maxHealth;

        if (hunger <= 0)
            hunger = 0;
        if (hunger >= maxHunger)
            hunger = maxHunger;

        if (thirst <= 0)
            thirst = 0;
        if (thirst >= maxThirst)
            thirst = maxThirst;

        // DEPLETION
        if (hunger <= 0)
            health -= hungerDamage * Time.deltaTime;

        if (thirst <= 0)
            health -= thirstDamage * Time.deltaTime;

        // HP10�Ŏ~�܂�
        //if (health < 10)
        //    health = 10;

        // DEPLETIONS
        if (hunger > 0)
            hunger -= hungerDepletion * Time.deltaTime;
        if (thirst > 0)
            thirst -= thirstDepletion * Time.deltaTime;
    }
}
