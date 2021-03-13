using UnityEngine;

using GameJam.FoodGun;

public class Player : MonoBehaviour
{
    #region Datamembers

    #region Editor Settings

    [SerializeField] private float speed;

    [SerializeField] private Gun gun;

    [SerializeField] private LayerMask groundMask;

    #endregion
    #region Private Fields

    private Rigidbody rigidbody;

    private Camera camera;

    #endregion

    #endregion


    #region Methods

    #region Unity Callbacks

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;
    }

    void Update()
    {
        var movemnet = new Vector3()
        {
            x = Input.GetAxisRaw("Horizontal"),
            z = Input.GetAxisRaw("Vertical")
        }
        .normalized;

        rigidbody.velocity = movemnet * speed;

        RotateGun();

        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot();
        }
    }

    #endregion

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
}
