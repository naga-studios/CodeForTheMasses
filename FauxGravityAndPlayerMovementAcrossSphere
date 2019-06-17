using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (ConstantForce))]
[RequireComponent(typeof (Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject world;
    [SerializeField] private float fauxGravityForce = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float worldRadius = 20f;

    private bool isGrounded = false;
    private float verticalMovement = 0f;

    private ConstantForce playerConstantForceComponent;
    private Vector3 worldCenter;
    private Rigidbody playerRigidbody;
    private Transform playerTransform;

    void Start()
    {
        worldCenter = world.transform.position;
        playerConstantForceComponent = this.GetComponent<ConstantForce>();
        playerRigidbody = this.GetComponent<Rigidbody>();
        playerTransform = this.GetComponent<Transform>();
        //worldRadius = world.GetComponent<MeshRenderer>().bounds.extents.magnitude;
    }

    void FixedUpdate()
    {
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        HandlePlayerMovement();
        ArtificialGravitationalPull();
        AdjustBodyWhileInAir();
    }

    private void HandlePlayerMovement()
    {
        verticalMovement = (Input.GetButton("Jump")) ? 1f : 0f;
        Vector3 moveDir = new Vector3(
            Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 
            verticalMovement * jumpForce * Time.deltaTime, 
            Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
        transform.Translate(moveDir);
    }

    private void ArtificialGravitationalPull()
    {
        Vector3 upDirection = (playerTransform.position - worldCenter).normalized;

        playerTransform.rotation = Quaternion.FromToRotation(playerTransform.up, upDirection) * playerTransform.rotation;

        if(GetDistanceBetweenSurfaceAndPlayer() > 0.7f)
        {
            playerTransform.Translate(new Vector3(0, -fauxGravityForce * Time.deltaTime, 0));
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    private void AdjustBodyWhileInAir()
    {
        if (!isGrounded)
        {
            Vector3 upDirection = (playerTransform.position - worldCenter).normalized;
            float distance = GetDistanceBetweenSurfaceAndPlayer();
            if(distance > 2)
            {
                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.FromToRotation(playerTransform.up, upDirection) * playerTransform.rotation, 1f * Time.deltaTime);
            }
            else
            {
                isGrounded = true;
            }
        }
    }

    private float GetDistanceBetweenSurfaceAndPlayer()
    {
        float distance = Vector3.Distance(playerTransform.position, worldCenter) - worldRadius;
        Debug.Log(distance);
        return distance;
    }
}
