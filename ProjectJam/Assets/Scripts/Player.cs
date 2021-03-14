using UnityEngine;

using GameJam.FoodGun;
using BarthaSzabolcs.RichTextHelper;
using BarthaSzabolcs.CommonUtility;
using System;

using GameJam;
using GameJam.AI;

public class Player : MonoBehaviour
{
    #region Datamembers

    #region Events

    public Action OnDeath;

    #endregion
    #region Enums

    private enum States { Move, Dash };

    #endregion
    #region Editor Settings

    [SerializeField] private ParticleSystem dust;
    [SerializeField] private float runSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private Gun gun;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private PlayerAnimationHelper animator;

    [SerializeField] private float dashTime;
    [SerializeField] private float dashRecharge;
    

    #endregion
    #region Private Properties

    private States State 
    { 
        get
        {
            return _state;
        }
        set
        {
            Debug.Log($"{name} changed state from {$"{_state}".Color(Color.magenta)} to {$"{value}".Color(Color.cyan)}");

            _state = value;
            switch (value)
            {
                case States.Move:
                    // dust.Stop();
                    dashRechargeTimer.Reset();
                    break;

                case States.Dash:
                    // dust.Play();
                    dashDurationTimer.Reset();
                    break;
                
                default:
                    break;
            }
        }
    }

    #endregion
    #region Backing Fields

    private States _state;

    #endregion
    #region Private Fields

    private Timer dashDurationTimer = new Timer();
    private Timer dashRechargeTimer = new Timer();

    private Rigidbody2D rigidbody;

    private Camera camera;

    private Vector3 dashVelocity;

    #endregion

    #endregion


    #region Methods

    #region Unity Callbacks

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;

        dashDurationTimer.Interval = dashTime;
        dashDurationTimer.Init();

        dashRechargeTimer.Interval = dashRecharge;
        dashRechargeTimer.Init();
    }

    void Update()
    {
        if (Time.deltaTime != 0)
        {
            StateLogic();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            OnDeath?.Invoke();
        }
    }

    #endregion

    private void StateLogic()
    {
        switch (State)
        {
            case States.Move:
                Run();
                RotateGun();
                ShootGun();
                break;

            case States.Dash:
                Dash();
                break;
        }
    }

    #region Movement

    private void Run()
    {
        
        dashRechargeTimer.Tick(Time.deltaTime);

        var movemnet = new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        }
        .normalized;

        if (dashRechargeTimer.Elapsed && Input.GetKeyDown("space"))
        {
            dashVelocity = movemnet * dashSpeed;
            rigidbody.velocity = movemnet * dashSpeed;

            State = States.Dash;
        }
        else
        {
            rigidbody.velocity = movemnet * runSpeed;
        }
    }

    private void Dash()
    {
        dashDurationTimer.Tick(Time.deltaTime);

        if (dashDurationTimer.Elapsed)
        {
            State = States.Move;
        }
    }

    #endregion
    #region Gun Handling

    private void ShootGun()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot();
        }
    }

    private void RotateGun()
    {
        var cursorPosition = camera.ScreenToWorldPoint(Input.mousePosition);

        var direction = cursorPosition - transform.position;
        direction.Scale(new Vector3(1, 1, 0));
        direction.Normalize();

        gun.transform.right = direction;

        animator.RefreshDirection(direction);
    }
    
    #endregion

    #endregion
}