    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private Dummy dum;

    [Header("HP")]
    [SerializeField] private Transform healthFieldDrawable;

    [SerializeField] private GameObject canv;

    [SerializeField] private float hp;

    private float maxHP;

    [Header("anim")]
    [SerializeField] private Animator an;

    [Header("Отталкивание")]
    [SerializeField] private float knockSpeedX;

    [SerializeField] private Rigidbody2D rb;

    private PlayerMovement pl;

    private float knockSpeedY = 1f;

    private void Start()
    {
        maxHP = hp;

        dum = gameObject.GetComponent<Dummy>();

        pl = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    public void Damage(float damage)
    {
        if (hp > damage)
        {
            hp -= damage;

            an.SetBool("isDamage", true);

            if (pl.isRight) rb.velocity = new Vector2(knockSpeedX, knockSpeedY);

            else rb.velocity = new Vector2(-knockSpeedX, knockSpeedY);
        }
        else
        {
            hp = 0f;

            KillDummy();
        }
    }

    private void KillDummy()
    {
        an.SetBool("isDead", true);

        dum.enabled = false;

        canv.SetActive(false);
    }

    private void EndPain()
    {
        an.SetBool("isDamage", false);
    }

    private void Update()
    {
        healthFieldDrawable.localScale = new Vector3(hp / maxHP, 1, 1);
    }
}
