using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PostProcessVolume post;
    private ChromaticAberration chrom;
    [SerializeField] private GameObject[] spr;


    public float healthPoint;
    private float maxHP;

    private bool isAlive;

    [Header("Animation")]

    [SerializeField] private Animator an;

    [Header("Skripts")]

    [SerializeField] private PlayerAttackSistem attack;

    [SerializeField] private PlayerMovement move;

    [Header("Phys")]
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        post.profile.TryGetSettings(out chrom);
        chrom.active = false;
        maxHP = healthPoint;
    }
    public void GetHealthPoint(float hp)
    {
        hp = healthPoint;
    }
    
    public void PlayerPain(float damage)
    {
        chrom.active = true;

        if (healthPoint > damage)
        {
            healthPoint -= damage;
            an.SetBool("isPain", true);
        }
        else
        {
            healthPoint = 0f;
            KillPLayer();
        }
    }


    private void KillPLayer()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);

        move.enabled = false;
        attack.enabled = false;

        isAlive = false;
        an.SetBool("isAlive", isAlive);

        Time.timeScale = 0.5f;

        spr[0].SetActive(false);
        spr[1].SetActive(true);

        Cursor.visible = true;
    }

    private void PlayerHill(float hpPlus)
    {
        if (hpPlus + healthPoint < maxHP) healthPoint += hpPlus;
        else healthPoint = maxHP;
    }
}
