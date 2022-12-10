using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squishy : Enemy
{
    public event OnSeePlayer _changeAttackingEnemy;
    public event OnSeePlayer _onEnemyDeath;

    private bool _getBack;

    private float _enemyCount;



    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void GetBack()
    {
        _getBack = true;

        if (currentDirection == -1)
        {
            ChangeDirection();
            ChangeRotation();
        }

        ChangeStoppingDistance(0);

        ChangeCanAttack(false);
    }

    protected override void Death()
    {
        base.Death();

        gameObject.GetComponent<Squishy>().enabled = false;

        _onEnemyDeath.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy") && _getBack)
            _enemyCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _getBack)
        {
            _enemyCount--;

            if (_enemyCount <= 0)
            {
                _getBack = false;
                ChangeDirection();
                ChangeRotation();
                Move(0);
                _changeAttackingEnemy.Invoke();
            }
        }
    }
}
