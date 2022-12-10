using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peh : Enemy
{
    public bool lol;
    protected override void Start()
    {
        base.Start();
        ChangeCanAttack(true);
    }
    

    protected override void Update()
    {
        base.Update();
        lol = GetCanAttack();
    }

    protected override void EndAttack()
    {
        isAttacking = false;
        enemyAnim.SetBool("isAttacking", false);
        GetPermissionCanMoveOrNot(true);
        //ChangeCanAttack(true);
    }

    protected override void Death()
    {
        base.Death();

        gameObject.GetComponent<Peh>().enabled = false;
    }
}
