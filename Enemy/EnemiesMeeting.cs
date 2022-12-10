using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesMeeting : MonoBehaviour
{
    [SerializeField] private Squishy _squishy;

    [SerializeField] private string _enemyTag;

    [SerializeField] private float _delayForDialog;
    [SerializeField] private float _delayBtwDialog;

    private bool _canEnterInDialogue = true;

    private void Update()
    {
        if (_squishy.GetSeePlayer())
            enabled = false;
    }

    private void EnemyScriptOff()
    {
        if (!_squishy.GetSeePlayer())
            _squishy.enabled = false;
    }

    private void EnemyScriptOn()
    {
        _squishy.enabled = true;

        _squishy.GetPermissionCanMoveOrNot(true);

        Invoke("GetPermissionToEnterInDialogue", _delayBtwDialog);
    }

    private  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _enemyTag && _canEnterInDialogue)
        {
            GetPermissionToEnterInDialogue();

            _squishy.GetPermissionCanMoveOrNot(false);

            Invoke("EnemyScriptOff", 0.5f);

            Invoke("EnemyScriptOn", _delayForDialog);
        }
    }

    private void GetPermissionToEnterInDialogue()
    {
        _canEnterInDialogue = _canEnterInDialogue == true ? false : true;
    }
}
