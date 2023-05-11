using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] GameObject cam;
    CinemachinePOV pov;

    [HideInInspector] public Rigidbody _playerRigidbody;

    [HideInInspector] public static bool canMove = true;
    [HideInInspector] public bool activateSpeedControl = true;
   
    private bool isRunning;
    private bool isSwiming;

    [Header("Speed Parametres\n")]
    [SerializeField] private float force = 30f;
    [SerializeField] private float groundDrag = 10f;
    [SerializeField] float rayOffset = .5f;
    [SerializeField] private float airMultiplyer;

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    public bool isWatered = false;

    public float lerpSpeed;

    private Vector3 moveDirection;

    [Header("Jump Parametres\n")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float playerHight;

    [SerializeField] private LayerMask whatIsGorund;
    [SerializeField] private LayerMask whatIsWater;

    private bool canJump = true;
    public bool isGrounded = true;

    [Header("Gravity Modifier\n")]
    public float normalGrav = -10f;

    public float currentGrav;

    private float oneUnit = 1f;
    private float halfUnit = .5f;
    private float dobleUnit = 2f;
    private float tenthOfUnit = .1f;
    private float normalForceMulti = 10f;
    private float runningForceMulti = 14f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerRigidbody = GetComponent<Rigidbody>();
        pov = cam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();
        Camera.main.layerCullSpherical = true;
        canMove = true;
        currentGrav = normalGrav;

        if (PlayerPrefs.HasKey("x"))
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y") + 2, PlayerPrefs.GetFloat("z"));
            pov.m_HorizontalAxis.Value = PlayerPrefs.GetFloat("hValue");
            pov.m_VerticalAxis.Value = PlayerPrefs.GetFloat("vValue");
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, cam.transform.eulerAngles.y, transform.eulerAngles.z);

        //Restricting the movement of the player with a boolean
        if (canMove)
        {
            PlayerInput();
            SpeedControl();
            
            if (isGrounded)
            {
                _playerRigidbody.drag = groundDrag;
                Physics.gravity = new Vector3(0, currentGrav, 0);
            }
            else
            {
                _playerRigidbody.drag = oneUnit;
                Physics.gravity = new Vector3(0, currentGrav * 2, 0);
            }


            if (!isWatered)
            {
                currentGrav = normalGrav;
            }

            pov.enabled = true;
        }
        else
        {
            pov.enabled = false;
            if(isGrounded) _playerRigidbody.velocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        //Restricting the movement of the player with a boolean
        if (canMove)
        {
            MovePlayer();
        }

        //Raycast that checks if the player is touching the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund) 
            || Physics.Raycast(new Vector3(transform.position.x + rayOffset, transform.position.y, transform.position.z), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund) 
            || Physics.Raycast(new Vector3(transform.position.x - rayOffset, transform.position.y, transform.position.z), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund) 
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + rayOffset), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - rayOffset), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund);

        Debug.DrawRay(new Vector3(transform.position.x + rayOffset, transform.position.y, transform.position.z), Vector3.down);
        Debug.DrawRay(new Vector3(transform.position.x - rayOffset, transform.position.y, transform.position.z), Vector3.down);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z + rayOffset), Vector3.down);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z - rayOffset), Vector3.down);
    }

    //All the inputs on whitch the player depends to move arround
    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump if the player is on the ground
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0))
        {
            if (canJump && isGrounded)
            {
                canJump = false;

                JumpMechanic();

                Invoke(nameof(JumpReset), jumpCooldown);
            }
            else if (isWatered)
            {
                currentGrav = Mathf.Lerp(currentGrav, 10, Time.deltaTime * lerpSpeed);
                isSwiming = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isSwiming = false;
            currentGrav = normalGrav;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isRunning = false;
        }
    }

    //All the diferents move speeds of the player depending on if is touching the ground, running, crouching, attatched to a wall, grappling or in free fall
    void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (isGrounded)
        {
            if (isRunning)
            {
                _playerRigidbody.AddForce(moveDirection.normalized * force * runningForceMulti, ForceMode.Force);
            }
            else
            {
                _playerRigidbody.AddForce(moveDirection.normalized * force * normalForceMulti, ForceMode.Force);
            }
        }
        else
        {
            _playerRigidbody.AddForce(moveDirection.normalized * force * (normalForceMulti - dobleUnit) * airMultiplyer, ForceMode.Force);
        }
    }

    //If the player isn't running the players max speed is clamped to a max value
    void SpeedControl()
    {
        if (activateSpeedControl)
        {
            Vector3 flatVel = new Vector3(_playerRigidbody.velocity.x, 0, _playerRigidbody.velocity.z);
            float newForce = force / dobleUnit;

            if (flatVel.magnitude > newForce && !isRunning)
            {
                Vector3 limitedVel = flatVel.normalized * newForce;
                _playerRigidbody.velocity = new Vector3(limitedVel.x, _playerRigidbody.velocity.y, limitedVel.z);
            }
        }
    }

    //When called this function adds an upwards force to the player
    void JumpMechanic()
    {
        _playerRigidbody.velocity = new Vector3(_playerRigidbody.velocity.x, 0f, _playerRigidbody.velocity.z);

        _playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //When called set the ability to jump to true
    void JumpReset()
    {
        canJump = true;
    }

    void OnTriggerStay(Collider other)
    {
        if ((whatIsWater.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            isWatered = true;
            if(!isSwiming) currentGrav = -5;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((whatIsWater.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            isWatered = false;
        }
    }

    public void CurrentPos()
    {
        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);
        PlayerPrefs.SetFloat("z", transform.position.z);

        PlayerPrefs.SetFloat("vValue", pov.m_VerticalAxis.Value);
        PlayerPrefs.SetFloat("hValue", pov.m_HorizontalAxis.Value);
    }
}