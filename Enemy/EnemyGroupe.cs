using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupe : MonoBehaviour
{
    [SerializeField] private List<Squishy> _enemies;

    [SerializeField] private float _stoppingDistanceForWaiting;
    [SerializeField] private float _stoppingDistanceForAttacking;

    [SerializeField] private int _attackingEnemy = -1;

    private void Awake()
    {
        foreach (var _enemy in _enemies)
        {
            _enemy.onSeePlayer += OnSeePlayer;
            _enemy._changeAttackingEnemy += ChangeAttackingEnemy;
            _enemy._onEnemyDeath += OnEnemyDeath;
        }
    }

    private void OnSeePlayer()
    {
        foreach (var _enemy in _enemies)
            _enemy.ChangeSeePlayer(true);

        ChangeAttackingEnemy();
    }

    private void ChangeAttackingEnemy()
    {
        var _currentEnemyIndex = 0;

        _attackingEnemy = _attackingEnemy + 1 >= _enemies.Count ? 0 : _attackingEnemy + 1;

        foreach (var _enemy in _enemies)
        {
            _enemy.ChangeStoppingDistance(_stoppingDistanceForWaiting + _enemy.gameObject.GetComponent<BoxCollider2D>().size.x * _currentEnemyIndex);

            _currentEnemyIndex++;
        }
            

        Attack();
    }

    private void Attack()
    {
        _enemies[_attackingEnemy].ChangeCanAttack(true);
        _enemies[_attackingEnemy].ChangeStoppingDistance(_stoppingDistanceForAttacking);
    }

    private void OnEnemyDeath()
    {
        var _currentEnemyIndex = 0;
        var _deadEnemyIndex = -1;

        if (_enemies.Count == 0)
            Destroy(gameObject);

        foreach (var _enemy in _enemies)
        {
            if (!_enemy.enabled)
                _deadEnemyIndex++;

            _currentEnemyIndex++;
        }
        Debug.Log(_deadEnemyIndex);
        _enemies[_deadEnemyIndex].gameObject.SetActive(false);

        _enemies.RemoveAt(_deadEnemyIndex);

        _attackingEnemy = _attackingEnemy + 1 >= _enemies.Count ? 0 : _attackingEnemy + 1;

        ChangeAttackingEnemy();
    }

  
}
