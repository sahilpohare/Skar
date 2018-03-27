using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Serializable]
    public class AdvancedSettings
    {
        public float shellOffset = .1f;
        public float groundCheckDistance = .06f;
        public LayerMask ignoredGrounds;
    }

    public AdvancedSettings advancedSettings_Access = new AdvancedSettings();

    public float speed = 5;
    public float jumpForce = 10;
    public int midAirJumps = 1;
    public float rotationSpeed = 15;
    public float groundedDrag = 10;
    [SerializeField] float timeTillNextJump = .5f;
    private float jumpTime;
    private float currentNumberOfJumps;

    CameraMovement cameraMovement_Access;

    Vector3 moveDirection;

    Rigidbody rigidbody;

    CapsuleCollider playerCollider;
    public bool onGround;
    public bool midairControl;

    // Use this for initialization
    void Start () {
        cameraMovement_Access = GetComponent<CameraMovement>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = groundedDrag;
        playerCollider = GetComponentInChildren<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        AirborneCheck();

        if(midairControl)
        {
            ExecuteMovement();
        }
        else
        {
            if(onGround)
            {
                ExecuteMovement();
            } 
        }
    }

    Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void GetMoveDirection(Vector2 input, Transform camAnchor)
    {
        moveDirection = camAnchor.forward * input.y + camAnchor.right * input.x;
    }

    void RotateToMoveDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed);
    }

    void Move(Vector3 direction)
    {
        if (rigidbody.velocity.magnitude < speed)
        {
            rigidbody.AddForce(direction * speed, ForceMode.Impulse);
        }
        else
        {
            rigidbody.velocity = new Vector3(direction.x * speed, rigidbody.velocity.y, direction.z * speed);
        }
    }

    void ExecuteMovement()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            GetMoveDirection(GetInput(), cameraMovement_Access.V_camAnchor);
            if (Input.GetButton("Horizontal") && Input.GetButton("Vertical"))
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
        if(Physics.SphereCast(transform.position, playerCollider.radius * (1 - advancedSettings_Access.shellOffset), Vector3.down, out hit, ((playerCollider.height/2) - playerCollider.radius) + advancedSettings_Access.groundCheckDistance, ~advancedSettings_Access.ignoredGrounds, QueryTriggerInteraction.Ignore))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    void AirborneCheck()
    {
        if(onGround)
        {
            rigidbody.drag = groundedDrag;
            currentNumberOfJumps = 0;
            Jump();
        }
        else
        {
            rigidbody.drag = 0;
            if (currentNumberOfJumps < midAirJumps)
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
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jumpTime = Time.time;
            currentNumberOfJumps++;
        }
    }
}
