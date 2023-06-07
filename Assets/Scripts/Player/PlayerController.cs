using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public static PlayerController instance;

    [SerializeField] GameObject cam;
    CinemachineFreeLook freeLookCam;

    [HideInInspector] public Rigidbody _playerRigidbody;
    public Animator _playerAnimator;
    public Animator _rotAnimator;
    public AudioSource _playerAudioSourceOneShot;
    public AudioSource _playerAudioSource;

    Vector2 lastDirection;

    [HideInInspector] public static bool canMove = true;
    [HideInInspector] public bool activateSpeedControl = true;
   
    private bool isRunning;
    private bool isSwiming;

    [Header("Speed Parametres\n")]
    public float force = 30f;
    [SerializeField] private float groundDrag = 10f;
    [SerializeField] float rayOffset = .5f;
    [SerializeField] private float airMultiplyer;

    public float horizontalInput;
    public float verticalInput;

    public bool isInWater = false;

    public float lerpSpeed;
    public float rotSpeed;

    public Vector3 moveDirection;

    [Header("Jump Parametres\n")]
    public float jumpForce = 10f;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float playerHight;

    [SerializeField] private LayerMask whatIsGorund;
    [SerializeField] private LayerMask whatIsWater;

    private bool canJump = true;
    public bool isGrounded = true;
    public bool isFalling = true;

    [Header("Gravity Modifier\n")]
    public float normalGrav = -10f;

    public float waterGrav = -3f;

    public float currentGrav;

    [Header("Interact parameters")]
    [SerializeField] private LayerMask whatIsItems;

    [SerializeField] float interactDist = 5f;

    private float oneUnit = 1f;
    private float halfUnit = .5f;
    private float dobleUnit = 2f;
    private float tenthOfUnit = .1f;
    private float normalForceMulti = 10f;
    private float runningForceMulti = 12f;

    public float x;
    public float y;

    private void Awake()
    {
        instance = this;

        _playerRigidbody = GetComponent<Rigidbody>();
        freeLookCam = cam.GetComponent<CinemachineFreeLook>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
       
        Camera.main.layerCullSpherical = true;
        canMove = true;
        currentGrav = normalGrav;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
        data.hValue = this.freeLookCam.m_XAxis.Value;
        data.vValue = this.freeLookCam.m_YAxis.Value;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
        this.freeLookCam.m_XAxis.Value = data.hValue;
        this.freeLookCam.m_YAxis.Value = data.vValue;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, freeLookCam.m_XAxis.Value, transform.eulerAngles.z);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Restricting the movement of the player with a boolean
        if (canMove)
        {
            PlayerInput();
            SpeedControl();
            CollectItem(ray);

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

            if (moveDirection != Vector3.zero)
            {
                if (isGrounded)
                {
                    _playerAudioSource.mute = false;
                }
                else if (!isGrounded)
                {
                    _playerAudioSource.mute = true;
                }
            }
            else
            {
                _playerAudioSource.mute = true;
            }

            if (!isInWater)
            {
                currentGrav = normalGrav;
            }

            freeLookCam.enabled = true;
        }
        else
        {
            freeLookCam.enabled = false;
            _playerRigidbody.velocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        //Raycast that checks if the player is touching the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund)
            || Physics.Raycast(new Vector3(transform.position.x + rayOffset, transform.position.y, transform.position.z), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund)
            || Physics.Raycast(new Vector3(transform.position.x - rayOffset, transform.position.y, transform.position.z), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + rayOffset), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - rayOffset), Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund);

        isFalling = Physics.Raycast(transform.position, Vector3.down, playerHight * halfUnit + tenthOfUnit, whatIsGorund);

        //Restricting the movement of the player with a boolean
        if (canMove)
        {
            MovePlayer();
        }
    }

    private void LateUpdate()
    {
        x = Mathf.Lerp(x, horizontalInput, Time.deltaTime * rotSpeed);
        y = Mathf.Lerp(y, verticalInput, Time.deltaTime * rotSpeed);

        _rotAnimator.SetFloat("Horizontal", x);

        _rotAnimator.SetFloat("Vertical", y);

        if (moveDirection != Vector3.zero)
        {
            _playerAnimator.SetBool("IsRunning", true);

            if (isRunning)
            {
                _playerAnimator.speed = 1.1f;
            }
            else
            {
                _playerAnimator.speed = 1f;
            }
        }
        else
        {
            _playerAnimator.SetBool("IsRunning", false);
        }

        if (_playerAnimator.GetBool("IsJumping") && canJump && isGrounded)
        {
            _playerAnimator.SetBool("IsJumping", false);
        }

        if (!isGrounded && isInWater)
        {
            _playerAnimator.SetBool("IsInWater", true);
        }
        else
        {
            _playerAnimator.SetBool("IsInWater", false);

            _playerAnimator.SetBool("IsFalling", false);
        }

        if (!isFalling)
        {
            _playerAnimator.SetBool("IsFalling", true);
        }
    }

    void CollectItem(Ray ray)
    {
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(ray, out hit, interactDist, whatIsItems))
            {
                ItemPickup itemPickup = hit.transform.GetComponent<ItemPickup>();

                itemPickup.PickUpItem(Inventory.instance);
            }
        }
    }

    //All the inputs on whitch the player depends to move arround
    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump if the player is on the ground
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0))
        {
            if (canJump && isGrounded && !isInWater)
            {
                canJump = false;

                JumpMechanic();

                Invoke(nameof(JumpReset), jumpCooldown);
            }

            if (isInWater)
            {
                currentGrav = Mathf.Lerp(currentGrav, Mathf.Abs(normalGrav / 2), Time.deltaTime * lerpSpeed);
                isSwiming = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isSwiming = false;
            currentGrav = normalGrav;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (isInWater)
            {
                currentGrav = Mathf.Lerp(currentGrav, normalGrav / 2, Time.deltaTime * lerpSpeed);
                isSwiming = true;
            }
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
        else if (isInWater)
        {
            _playerRigidbody.AddForce(moveDirection.normalized * force * (normalForceMulti / dobleUnit) * airMultiplyer, ForceMode.Force);
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
    public void JumpMechanic()
    {
        AudioManager.instance.PlayOneShot(_playerAudioSourceOneShot, 1);

        _playerAnimator.SetBool("IsJumping", true);

        _playerAnimator.Play("Jump");

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
            if (!isSwiming) currentGrav = waterGrav;
            isInWater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((whatIsWater.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            isInWater = false;
        }
    }
}