using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    public static MovementController Instance { get; private set; }

    [InspectorName("Horizontal Inputs")]
    private float horizontalInput, horizontalJoystick;
    [InspectorName("Vertical Inputs")]
    private float verticalInput, verticalJoystick;
    
    internal static bool isInTheCar;

    [Header("Speed Variables")]
    [SerializeField] private readonly float carSpeed = 10f;
    [SerializeField] private float _playerSpeed = 7.5f;
    [SerializeField] private float _turnSpeed = 75f;

    [Header("Components")]
    private Joystick _joystick;
    private CarController _carController;
    private Rigidbody _rb;
    private CapsuleCollider _playerCollider;
    private MeshRenderer _playerMesh;

    protected void Awake()
    {
        Instance = this;

        _rb = GetComponent<Rigidbody>();
        _joystick = FindObjectOfType<Joystick>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _playerMesh = GetComponent<MeshRenderer>();
    }

    protected void Update()
    {
        if (!isInTheCar) //control the player   
        {
            MoveWithKeyboard(gameObject, _playerSpeed);
            MoveWithJoystick(gameObject, _playerSpeed);
        }
        else //control the car 
        {
            MoveWithKeyboard(_carController.gameObject, carSpeed);
            MoveWithJoystick(_carController.gameObject, carSpeed);
        }
    }

    protected void OnCollisionEnter(Collision collision) //entering the car
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            _carController = collision.gameObject.GetComponent<CarController>(); //caching the car 

            if (!_carController.isCanRideable)
            {
                return;
            }

            PlayerDisappearing();
            KickTheCarFromQueue(collision, _carController); //arabayý sýradan atma
            isInTheCar = true;
        }
    }
    private void MoveWithKeyboard(GameObject willBeControled, float speed)
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (isInTheCar)
        {
            transform.position = _carController.transform.position;

            willBeControled.transform.Translate(speed * Time.deltaTime * verticalInput * Vector3.forward.normalized);

            if (verticalInput > 0) //ileri giderken saða sola dönmesi
            {
                willBeControled.transform.Rotate(horizontalInput * Time.deltaTime * _turnSpeed * Vector3.up.normalized);
            }
            else if (verticalInput < 0) //geriye giderken ters þekilde dönmesi
            {
                willBeControled.transform.Rotate(horizontalInput * Time.deltaTime * _turnSpeed * Vector3.down.normalized);
            }
        }
        else
        {
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

            willBeControled.transform.Translate(speed * Time.deltaTime * movement.normalized);
        }
    }
    private void MoveWithJoystick(GameObject willBeControled, float speed)
    {
        horizontalJoystick = _joystick.Horizontal;
        verticalJoystick = _joystick.Vertical;

        if (isInTheCar) //car control   
        {
            float angle = Mathf.Atan2(horizontalJoystick, verticalJoystick) * Mathf.Rad2Deg; //rotation setting
            Vector3 movement = new Vector3(horizontalJoystick, 0, verticalJoystick);

            willBeControled.transform.Translate(speed * Time.deltaTime * movement.normalized, Space.World);

            if (horizontalJoystick != 0) //rotation definining
            {
                willBeControled.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            }
        }
        else //player control 
        {
            Vector3 movement = new Vector3(horizontalJoystick, 0, verticalJoystick);

            willBeControled.transform.Translate(speed * Time.deltaTime * movement.normalized);
        }
    }
    private void KickTheCarFromQueue(Collision collision, CarController car) //leaving from the queue with the car
    {
        WaitingDetection.Instance.DequeueCar(collision.gameObject.GetComponent<CarController>());
        Destroy(car.Yelling);
    }
    public void PlayerAppearing()
    {
        _playerCollider.enabled = true;
        _playerMesh.enabled = true;
        transform.position = _carController.transform.position + new Vector3(-2.5f, 0, 0);
        isInTheCar = false;
        print("Player Appeared");
    } 
    public void PlayerDisappearing()
    {
        _playerCollider.enabled = false;
        _playerMesh.enabled = false;
    }
    public void FreezeThePlayer()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void ReleaseThePlayer()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
