using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] private Animator textAnim;

    [SerializeField] private string nameAnim;

    [SerializeField] private PlayerMovement pl;

    private void Start()
    {
        pl = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (Input.GetButtonDown("Dialog")))
        {
            DialogStart(pl, nameAnim);
        }
    }
    private void DialogStart(PlayerMovement pl, string name)
    {
        pl.canRun = false;
        textAnim.SetBool(name, true);
    }
    private void DialogEnd(PlayerMovement pl)
    {
        pl.canRun = true;
    }
    
    
}
