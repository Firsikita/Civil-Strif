using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField] private Rigidbody2D rb;// rb игрока

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpVelocity;

    private float movementDirection;

    public bool canRun = true;

    [Space]
    [Header("Player Animator")]

    [SerializeField] private Animator an;

    [SerializeField] private Transform drawablePart; //отрисовываемые части игрока

    public bool isRight; //направление игрока как булевая переменная

    private bool isWalking;

    [Space]
    [Header("Collision")]

    [SerializeField] private bool isWall;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    [SerializeField] private float grRad;//радиус groundCheck
    [SerializeField] private float wallCheckDist;
    [SerializeField] private float wallSlideSpeed;

    [SerializeField] private LayerMask whichGround;//для определения layer-а в который упирается игрок

    private bool isWallSliding;
    private bool isGround;

    [Space]
    [Header("Dash")]//рывок/перекат

    [SerializeField] private CameraShake cam;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    

    [SerializeField] private bool isDashing;

    public bool canDash;

    //счётчики для рывка
    private float dashTimer;

    public float dashReloadTimer;
    public float dashTimeToReload;

    private void Update()
    {
        InputCheck();

        MovementDirectionCheck();

        AnimatorUpdate();

        CheckisWallSliding();

        DashTimer();
    }

    private void FixedUpdate()
    {
        Movement();

        CheckCollision();

        Dash();
    }

    private void InputCheck()
    {
        MovementInput();

        JumpInput(isWallSliding);

        DashInput();
    }

    //методы для реализации движения
    #region Movement
    private void MovementInput()
    {
        if(canRun) movementDirection = Input.GetAxisRaw("Horizontal");
    }

    private void MovementDirectionCheck()
    {
        if (isRight && movementDirection < 0)
            Flip();
        else if (!isRight && movementDirection > 0)
            Flip();

        if (rb.velocity.x >  0.05f || rb.velocity.x < - 0.05f)
            isWalking = true;
        else 
            isWalking = false;
    }

    private void Movement()
    {
        rb.velocity = new Vector2(movementSpeed * movementDirection, rb.velocity.y);

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }
    #endregion

    //методы для реализации анимаций и отображения персонажа
    #region Animation
    private void AnimatorUpdate()
    {
        an.SetBool("IsWalking", isWalking);
        an.SetBool("isGround", isGround);
        an.SetFloat("yVelocity", rb.velocity.y);
        an.SetBool("isSliding", isWallSliding);
        an.SetBool("isWall", isWall);
    }

    private void Flip()
    {
        isRight = !isRight;

        drawablePart.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion

    //методы для реализации всех видов прыжков (коллизия со слоями, прыжки, сползания)
    #region Jump
    private void CheckCollision()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, grRad, whichGround);

        isWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDist, whichGround);
    }

    private void JumpInput(bool wall)
    {
        if (Input.GetButtonDown("Jump") && (isGround || isWall))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }

    private void CheckisWallSliding()
    {
        if (isWall && !isGround)
            isWallSliding = true;
        else
            isWallSliding = false;
    }
    #endregion

    //методы для реализации рывка/переката  
    #region Dash
    private void DashInput()
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {
            isDashing = true;

            canDash = false;

           // cam.StartShake();
        }
    }

    private void Dash()
    {
        if (isDashing)
        {
            rb.AddRelativeForce(new Vector2(movementDirection * dashSpeed, dashSpeed/15));
        }
    }

    private void DashTimer()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashTime)
            {
                isDashing = false;
                dashTimer = 0;
            }
        }
        if (!isDashing && !canDash)
        {
            dashReloadTimer += Time.deltaTime;

            if (dashReloadTimer >= dashTimeToReload)
            {
                canDash = true;
                dashReloadTimer = 0;
            }
        }
    }
    #endregion

    //отрисовка интерфейса для дебага
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, grRad);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDist, wallCheck.position.y, wallCheck.position.z));
    }
}
