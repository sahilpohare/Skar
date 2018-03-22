using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 5;
    public float jumpForce = 10;
    public float rotationSpeed = 15;

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;

    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform V_camAnchor;
    [SerializeField] private Transform H_camAnchor;
    public float cameraDistance = 10;

    Vector3 moveDirection;

    Rigidbody rigidbody;
    CapsuleCollider playerCollider;

    // Use this for initialization
    void Start () {
        if(playerCam == null)
        {
            playerCam = Camera.main;
        }
        V_camAnchor.position = transform.position;
        rigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponentInChildren<CapsuleCollider>();
    }

    private void Update()
    {
        V_camAnchor.position = transform.position;
        RotateCamera(GetMouseMovement());
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            GetMoveDirection(GetInput());
            if(Input.GetButton("Horizontal") && Input.GetButton("Vertical"))
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
        Jump();
        Debug.Log(rigidbody.velocity.magnitude);
    }

    Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void GetMoveDirection(Vector2 input)
    {
        moveDirection = V_camAnchor.forward * input.y + V_camAnchor.right * input.x;
    }

    Vector2 GetMouseMovement()
    {
        return new Vector2(Input.GetAxis("Mouse X") * XSensitivity, Input.GetAxis("Mouse Y") * YSensitivity);
    }

    void RotateToMoveDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed);
    }

    void CheckGround()
    {

    }

    void Move(Vector3 direction)
    {
        if(rigidbody.velocity.magnitude < speed)
        {
            rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
        }
        else
        {
            rigidbody.velocity = direction * speed;
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void RotateCamera(Vector2 mouseInput)
    {
        V_camAnchor.Rotate(0, mouseInput.x, 0, Space.Self);
        H_camAnchor.Rotate(-mouseInput.y, 0, 0, Space.Self);
    }
}
