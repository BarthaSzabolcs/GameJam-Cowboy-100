using UnityEngine;

using GameJam.FoodGun;
using BarthaSzabolcs.RichTextHelper;
using BarthaSzabolcs.CommonUtility;

public class Player : MonoBehaviour
{
    #region Datamembers

    #region Enums

    private enum States { Move, Dash };

    #endregion
    #region Editor Settings

    [SerializeField] private float runSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private Gun gun;
    [SerializeField] private LayerMask groundMask;

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
                    dashRechargeTimer.Reset();
                    break;

                case States.Dash:
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

    private Rigidbody rigidbody;

    private Camera camera;

    private Vector3 dashVelocity;

    #endregion

    #endregion


    #region Methods

    #region Unity Callbacks

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;

        dashDurationTimer.Interval = dashTime;
        dashDurationTimer.Init();

        dashRechargeTimer.Interval = dashRecharge;
        dashRechargeTimer.Init();
    }

    void Update()
    {
        StateLogic();
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

        var movemnet = new Vector3()
        {
            x = Input.GetAxisRaw("Horizontal"),
            z = Input.GetAxisRaw("Vertical")
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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, groundMask))
        {
            var horizontalScale = new Vector3(1, 0, 1);

            var playerPosition = transform.position;
            playerPosition.Scale(horizontalScale);

            var cursorPosition = hitInfo.point;
            cursorPosition.Scale(horizontalScale);

            var direction = cursorPosition - playerPosition;

            gun.transform.forward = direction.normalized;
        }

    }
    
    #endregion

    #endregion
}