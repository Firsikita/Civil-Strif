using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PlayerAttackSistem : MonoBehaviour
{
    [SerializeField] private PostProcessVolume post;
    private ChromaticAberration chrom;

    [SerializeField] private PlayerMovement pl;

    [SerializeField] private Rigidbody2D rb;

    [Header("Combat")]
    [SerializeField] private bool isCombat;

    [SerializeField] private float inputTimer;
    [SerializeField] private float attackRad;
    [SerializeField] private float attackDamage;

    [SerializeField] private Transform atHitBoxPosition;

    [SerializeField] private LayerMask isDamageable;



    private bool gotInput;
    private bool isAttacking;

    private float lastInpTime = Mathf.NegativeInfinity;

    [Header("Animation")]

    [SerializeField] private Animator an;

    [SerializeField] private CameraShake cam;

    private void Start()
    {
        post.profile.TryGetSettings(out chrom);
        chrom.active = false;
    }

    private void Update()
    {
        CheckAttackInput();
        AttacksChecker();
    }


    private void CheckAttackInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isCombat)
            {
                gotInput = true;
                lastInpTime = Time.time;
                pl.canRun = false;
                rb.simulated = false;
            }
        }
    }

    private void AttacksChecker()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                an.SetBool("IsAttack", isAttacking);
            }
        }

        if (Time.time > lastInpTime + inputTimer)
        {
            gotInput = false;
        }
    }


    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(atHitBoxPosition.position, attackRad, isDamageable);
        pl.canRun = true;
        rb.simulated = true;
        foreach (Collider2D col in detectedObjs)
        {
            col.transform.SendMessage("Damage", attackDamage);
            cam.StartShake();
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        an.SetBool("IsAttack", isAttacking);
    }

    private void EndPain()
    {
        an.SetBool("isPain", false);
        chrom.active = false;
        pl.canRun = true;
        rb.simulated = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(atHitBoxPosition.position, attackRad);
    }
}
