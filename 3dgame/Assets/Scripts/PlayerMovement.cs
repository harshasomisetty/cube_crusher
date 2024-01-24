using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;


    [SerializeField] AudioSource moveSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        groundCheck = transform.Find("Ground Check");

        if (groundCheck == null)
        {
            Debug.LogError("Ground Check transform not found. Make sure the object is named correctly and is a child of the Player.");
        }

        int groundLayer = LayerMask.NameToLayer("Ground");
        ground = 1 << groundLayer;

    }

    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);

        if ((Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0) && !moveSound.isPlaying)
        {
            moveSound.Play();
        }

        if (Input.GetButtonDown("Jump") && isGrounded())
        {

            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
        }
    }

    bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
        // return true;
    }
}
