using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public delegate void OnSeePlayer();

    public event OnSeePlayer onSeePlayer;

    [Header("ИИ")]
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform _transform;
    [SerializeField] private Transform enemyEye;
    [SerializeField] private Transform player;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float patrolArea;
    [SerializeField] private float speed;  
    [SerializeField] private float pursuitSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float delayBeforeRotation;
    [SerializeField] private float seeDistance;
    [SerializeField] private float seeDistanceBack;

    [SerializeField] private string playerTag;

    [SerializeField] private float stoppingDistance = 2;
    private float currentPoint;

    public int currentDirection = 1;

    [SerializeField] private bool canMoving = true;
    private bool dashToPlayer = true;
    private bool canAttack;
    private bool seePlayer;

    [Header("Анимации")]
    [SerializeField] protected Animator enemyAnim;

    [Header("Сиситема атаки")]
    [SerializeField] private Transform attackHitBoxPos;

    [SerializeField] private float attackRad;
    [SerializeField] private float damage;
    [SerializeField] private float speedAttack;

    protected bool isAttacking;

    [Header("Система здоровья")]
    [SerializeField] private Transform healthFieldDrawable;

    [SerializeField] private GameObject canv;

    [SerializeField] private float health;
    private float healthMax;

    private bool isHearting;
    [Header("Отталкивание")]
    [SerializeField] private float knockSpeedX;
    [SerializeField] private float knockSpeedY = 1f;

    private PlayerMovement plMove;

    protected virtual void Start()
    {
        enemyEye.position = new Vector3(_transform.position.x - seeDistanceBack, _transform.position.y, _transform.position.z);

        currentPoint = gameObject.transform.position.x + patrolArea / 2 * currentDirection;

        player = GameObject.FindGameObjectWithTag(playerTag).transform;

        plMove = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerMovement>();

        healthMax = health;

        groundCheck.parent = null;
    }

    protected virtual void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (!seePlayer)
            Patroll();
        else
            PursuitPlayer();
        CalmPain();
    }

    #region PursuitPlayer

    private void PursuitPlayer()
    {
        CheckForRotation();

        var _collider = GetRaycastHit(stoppingDistance).collider;

        if (_collider == null)
        {
            GetPermissionCanMoveOrNot(true);
        }
        else if (_collider.CompareTag(playerTag))
        {
            if (canAttack)
                GetPermissionCanMoveOrNot(false);
                StartAttack();
        }

        if (stoppingDistance < 4 && stoppingDistance != 0 && _collider == null)
            dashToPlayer = true;

        if (dashToPlayer && stoppingDistance < 4)
            Dash(_collider);
        else
            Move(GetSpeed(pursuitSpeed));
    }

    private void Dash(Collider2D _collider)
    {
        if (dashToPlayer && stoppingDistance < 4)
        {
            Move(GetSpeed(dashSpeed));

            Invoke("OffDash", 0.2f);
        }
    }

    private void OffDash()
    {
        dashToPlayer = false;
    }

    private void CheckForRotation()
    {
        var _playerPosX = player.position.x;
        var _enemyPosX = _transform.position.x;

        if (((_playerPosX < _enemyPosX && currentDirection == 1) || (_playerPosX > _enemyPosX && currentDirection == -1)))
        {
            ChangeDirection();
            ChangeRotation();
        }
    }

    
   

    private void CheckForPlayer()
    {
        if (GetRaycastHit(seeDistance + seeDistanceBack).collider != null)
        {
            seePlayer = true;

            onSeePlayer?.Invoke();
        }
    }

    #endregion

    #region Patroll

    private void Patroll()
    {
        var _currentPositionX = _transform.position.x;

        CheckForPlayer();

        Move(GetSpeed(speed));

        if (_currentPositionX > currentPoint - 0.2 && _currentPositionX < currentPoint + 0.2)
            ChangePoint();
    }

    public void ChangePoint()
    {
        currentPoint += patrolArea * (-currentDirection);

        GetPermissionCanMoveOrNot(false);

        Invoke("ChangeDirection", delayBeforeRotation);

        Invoke("ChangeRotation", delayBeforeRotation);
    }

    public void ChangeDirection()
    {
        currentDirection *= -1;
    }

    public void ChangeRotation()
    {
        gameObject.transform.Rotate(0, gameObject.transform.rotation.y + 180 * currentDirection, 0);

        if (!seePlayer)
            GetPermissionCanMoveOrNot(true);
    }

    #endregion

    #region GetOrSetSomeParameters

    private RaycastHit2D GetRaycastHit(float _distance)
    {
        return Physics2D.Raycast(enemyEye.position, new Vector3(180 * currentDirection, 0, 0), _distance, layerMask);
    }

    private float GetSpeed(float _speed)
    {
        if (canMoving)
            return _speed;
        return 0;
    }

    protected bool GetCanAttack()
    {
        return canAttack;
    }

    public void GetPermissionCanMoveOrNot(bool _canMove)
    {
        canMoving = _canMove;
    }

    public bool GetSeePlayer()
    {
        return seePlayer;
    }

    public void ChangeCanAttack(bool _can)
    {
        canAttack = _can;
    }

    public void ChangeSeePlayer(bool _sawPlayer)
    {
        seePlayer = _sawPlayer;
    }

    public void ChangeStoppingDistance(float _stopDist)
    {
        stoppingDistance = _stopDist;
    }

    public void SetHealth(float _damage)
    {
        health -= _damage;
    }

    #endregion

    protected void Move(float _speed)
    {
        rb.velocity = new Vector2(_speed * currentDirection,rb.velocity.y);
        //Debug.Log(_rb.velocity.y);
    }  

    #region AttackSistem
    private void CheckAttackHitBox()
    {
        Collider2D detectedObj = Physics2D.OverlapCircle(attackHitBoxPos.position, attackRad, layerMask);
        detectedObj.transform.SendMessage("PlayerPain", damage);

    }

    private void StartAttack()
    {
        //Move(0);
        isAttacking = true;
        enemyAnim.SetBool("isAttacking", isAttacking);
    }
    protected virtual void EndAttack()
    {
        isAttacking = false;
        enemyAnim.SetBool("isAttacking", isAttacking);
        GetPermissionCanMoveOrNot(true);
    }


    #endregion

    #region HealthSistem

    private void CalmPain()
    {
        healthFieldDrawable.localScale = new Vector3(health / healthMax, 1, 1);
        if (isHearting)
        {
            if (plMove.isRight) rb.velocity = new Vector2(knockSpeedX, knockSpeedY);

            else rb.velocity = new Vector2(-knockSpeedX, knockSpeedY);
        }
    }
    public void Damage(float damage)
    {
        if (health > damage)
        {
            health -= damage;

            isHearting = true;
            enemyAnim.SetBool("isDamage", isHearting);

            GetPermissionCanMoveOrNot(false);
            
        }
        else
        {
            health = 0f;

            Death();
        }
    }

    private void EndPain()
    {
        isHearting = false;
        enemyAnim.SetBool("isDamage", isHearting);

        GetPermissionCanMoveOrNot(true);
    }

    protected virtual void Death()
    {
        enemyAnim.SetBool("isDead", true);

        canv.SetActive(false);

        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackHitBoxPos.position, attackRad);
        Gizmos.DrawWireSphere(new Vector2(groundCheck.position.x + patrolArea/2, groundCheck.position.y), 0.3f);
        Gizmos.DrawWireSphere(new Vector2(groundCheck.position.x - patrolArea / 2, groundCheck.position.y), 0.3f);

    }
}
