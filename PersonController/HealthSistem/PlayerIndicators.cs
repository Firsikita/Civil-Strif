using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicators : MonoBehaviour
{
    [SerializeField] private Transform healthFieldDrawable;
    [SerializeField] private Transform staminaFieldDrawable;

    private float hp;
    private float maxHP;
    private float stamina;
    private float staminaMax;

    public PlayerHealth playerHealth;
    public PlayerMovement playerMove;


    private void Start()
    {
        maxHP = playerHealth.healthPoint;
        staminaMax = playerMove.dashTimeToReload;
    }

    private void Update()
    {
        stamina = playerMove.dashReloadTimer;
        hp = playerHealth.healthPoint;
        DrawIndicators();
    }

    private void DrawIndicators()
    {
        healthFieldDrawable.localScale = new Vector2(hp/maxHP, 1);

        if(!playerMove.canDash) staminaFieldDrawable.localScale = new Vector2(stamina / staminaMax, 1);
        else staminaFieldDrawable.localScale = new Vector2(staminaMax / staminaMax, 1);

    }
}
