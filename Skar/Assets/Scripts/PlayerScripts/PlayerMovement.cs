using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StatesManager))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour {

    [Serializable]
    public class InputSettings
    {
        public bool showCursor;
        public CursorLockMode lockState = CursorLockMode.Locked;
        public float horizontal;
        public float vertical;
        public float mouseX;
        public float mouseY;

        public void CursorSettings(bool visibility, CursorLockMode lockState)
        {
            Cursor.lockState = lockState;
            Cursor.visible = visibility;
        }

        public Vector2 GetInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        public void GetAxis()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        public Vector3 GetMoveDirection(Vector2 input, Transform camAnchor)
        {
            Vector3 v;
            Vector3 h;
            v = camAnchor.forward;
            h = camAnchor.right;
            v.y = 0;
            h.y = 0;
            return (v * input.y + h * input.x).normalized;
        }
    }

    [Serializable]
    public class MovementSettings
    {
        public float speed = 5;
        public float jumpForce = 10;
        public int midAirJumps = 1;
        public float rotationSpeed = .4f;
        public float groundedDrag = 10;
        public bool midairControl;
    }

    [SerializeField] float timeTillNextJump = .5f;
    private float jumpTime;
    private float currentNumberOfJumps;

    [Serializable]
    public class AdvancedSettings
    {
        public float shellOffset = .1f;
        public float groundCheckDistance = .06f;
        public LayerMask ignoredGrounds;
    }

    public InputSettings inputSettings = new InputSettings();
    public MovementSettings movementSettings = new MovementSettings();
    public AdvancedSettings advancedSettings = new AdvancedSettings();
    public StatesManager states;
    [Range(0,1)]

    Rigidbody rigidbody;
    CapsuleCollider playerCollider;
    [SerializeField] Transform colliderCenter;
    private bool onGround;


    // Use this for initialization
    void Start () {
        inputSettings.CursorSettings(inputSettings.showCursor, inputSettings.lockState);
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = movementSettings.groundedDrag;
        playerCollider = GetComponentInChildren<CapsuleCollider>();
        colliderCenter.localPosition = playerCollider.center;
        states = GetComponent<StatesManager>();
    }

    private void Update()
    {
        inputSettings.CursorSettings(inputSettings.showCursor, inputSettings.lockState);
    }

    private void FixedUpdate()
    {
        inputSettings.GetAxis();
        CheckGround();
        AirborneCheck();

        if(states.CanMove)
        {
            if (movementSettings.midairControl)
            {
                ExecuteMovement();
            }
            else
            {
                if (onGround)
                {
                    ExecuteMovement();
                }
            }
        }
    }

    void RotateToMoveDirection(Vector3 direction)
    {
        Quaternion desiredRot = Quaternion.LookRotation(direction);
	    transform.rotation = Quaternion.Slerp(transform.rotation,desiredRot, movementSettings.rotationSpeed);
    }

    void Move(Vector3 direction)
    {
        if (rigidbody.velocity.magnitude < movementSettings.speed)
        {
            rigidbody.AddForce(direction * movementSettings.speed * rigidbody.mass, ForceMode.Impulse);
        }
        else
        {
            rigidbody.velocity = new Vector3(direction.x * movementSettings.speed, rigidbody.velocity.y, direction.z * movementSettings.speed);
        }
    }

    void ExecuteMovement()
    {
        Vector3 moveDirection;
        if (Mathf.Abs(inputSettings.horizontal) > 0 || Mathf.Abs(inputSettings.vertical) > 0)
        {
            moveDirection = inputSettings.GetMoveDirection(inputSettings.GetInput(), Camera.main.transform);
            if (Mathf.Abs(inputSettings.horizontal) > 0 && Mathf.Abs(inputSettings.vertical) > 0)
            {
                RotateToMoveDirection(moveDirection);
                Move(moveDirection * Mathf.Sin(Mathf.Deg2Rad * 45));
            }
            else
            {
                RotateToMoveDirection(moveDirection);
                Move(moveDirection);
            }
        }
    }

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.SphereCast(colliderCenter.position, playerCollider.radius * (1 - advancedSettings.shellOffset), Vector3.down, out hit, ((playerCollider.height / 2) - playerCollider.radius) + advancedSettings.groundCheckDistance, ~advancedSettings.ignoredGrounds, QueryTriggerInteraction.Ignore))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        states.isGrounded = onGround;
    }

    void AirborneCheck()
    {
        if(onGround)
        {
            rigidbody.drag = movementSettings.groundedDrag;
            currentNumberOfJumps = 0;
            Jump();
        }
        else
        {
            rigidbody.drag = 0;
            if (currentNumberOfJumps < movementSettings.midAirJumps)
            {
                if(Time.time > jumpTime + timeTillNextJump)
                {
                    Jump();
                }
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(transform.up * movementSettings.jumpForce * rigidbody.mass, ForceMode.Impulse);
            jumpTime = Time.time;
            currentNumberOfJumps++;
        }
    }
} 