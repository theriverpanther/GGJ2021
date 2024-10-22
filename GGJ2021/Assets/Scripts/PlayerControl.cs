﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject playerModel;

    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    float jumpForceMag = 2.5f;
    [SerializeField]
    float jumpForceVerticalMod = 1.5f;
    [SerializeField]
    bool variableJumpEnabled = false;
    [SerializeField]
    float initJumpForceMag = 1f;
    [SerializeField]
    float holdJumpForceMag = 0.1f;
    [SerializeField]
    float verticalJumpForce = 3.75f;
    [SerializeField]
    float maxJumpHoldDuration = 0.5f;

    float holdDuration = 0f;
    Vector3 jumpDirection = Vector3.forward;

    Rigidbody rigidbody;
    Collider collider;

    public List<AudioClip> gameSounds;

    public AudioSource camSource;
    public AudioSource playerSource;
    public AudioSource roombaSource;

    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the rigidbody and collider
        rigidbody = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {
        // -- Jump stuff --

        // If the user lets go of jump, ensure they can't press it again afterwards
        if (Input.GetKeyUp(jumpKey))
        {
            holdDuration = 100f;
        }

        // If the user is holding jump in the allowed time period, apply a force to them
        if (holdDuration <= maxJumpHoldDuration && Input.GetKey(jumpKey))
        {
            holdDuration += Time.deltaTime;
        }

        // If the user is on the ground, has a small y velocity, and is pressing the jump key, jump
        if ((IsGroundedV3() || rigidbody.velocity.magnitude < 0.005f) && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Add gravity to the player
        rigidbody.AddForce(Physics.gravity * 0.1f * rigidbody.mass);

        // If the player is holding the jump button within the duration, add more force
        if (holdDuration <= maxJumpHoldDuration && Input.GetKey(jumpKey))
        {
            rigidbody.AddForce(jumpDirection * holdJumpForceMag);
        }
    }

    void Jump()
    {
        // Play a jump sound
        playerSource.PlayOneShot(gameSounds[0]);

        // Get the direction the main camera is facing
        Vector3 lookDir = Camera.main.transform.forward;

        // Flatten the y value
        lookDir.y = 0;

        // Normalize the look direction
        lookDir = lookDir.normalized;

        jumpDirection = lookDir;

        Vector3 jumpForce;

        if (variableJumpEnabled)
        {
            jumpForce = jumpDirection * initJumpForceMag;

            holdDuration = 0;
        }
        else
        {
            jumpForce = jumpDirection * jumpForceMag;
        }

        jumpForce.y = verticalJumpForce;

        // Apply the jumping force to the player.
        rigidbody.AddForce(jumpForce);

        // Rotate the key randomly in air
        Vector3 torque = new Vector3();

        torque.x = Random.Range(-jumpForceMag, jumpForceMag);
        torque.y = Random.Range(-2 * jumpForceMag, 2 * jumpForceMag);
        torque.z = Random.Range(-jumpForceMag, jumpForceMag);

        rigidbody.AddTorque(torque);
    }

    // Version 1 of ground check
    bool IsGrounded()
    {
        float groundDist;

        if (Mathf.Abs(transform.rotation.eulerAngles.z) < 45)
        {
            groundDist = collider.bounds.extents.y;
        }
        else
        {
            groundDist = collider.bounds.extents.x;
        }

        return Physics.Raycast(collider.transform.position, -Vector3.up, groundDist + 0.01f);
    }

    // Version 2 of ground check
    bool TheCoolerIsGrounded()
    {
        Vector3 downPoint = new Vector3(
            collider.bounds.center.x,
            -500,
            collider.bounds.center.z);

        Vector3 playerBottom = collider.ClosestPoint(downPoint);

        playerBottom.y += 0.001f;

        return Physics.Raycast(playerBottom, -Vector3.up, 0.002f);
    }

    // Version 3 of ground check
    // Use the player's boundaries to check if the player is on the ground
    bool IsGroundedV3()
    {
        Vector3 downPoint = new Vector3(
            collider.bounds.center.x,
            -500,
            collider.bounds.center.z);

        Vector3 playerBottom = collider.ClosestPoint(downPoint);

        playerBottom.y += 0.001f;

        Vector3 upPoint = new Vector3(
            collider.bounds.center.x,
            500,
            collider.bounds.center.z);

        Vector3 playerTop = collider.ClosestPoint(upPoint);

        Vector3 playerCenter = (playerBottom + playerTop) / 2f;

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log(playerTop.y);
        //    Debug.Log(playerCenter.y);
        //    Debug.Log(playerBottom.y);
        //}

        // All layers except for the player
        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        return Physics.Raycast(playerBottom, -Vector3.up, 0.002f, layerMask)
            || Physics.Raycast(playerTop, -Vector3.up, 0.012f, layerMask)
            || Physics.Raycast(playerCenter, -Vector3.up, 0.007f, layerMask);
    }

    // Whenever the collider is entered, play a random collision sound
    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            playerSource.PlayOneShot(gameSounds[Random.Range(1, gameSounds.Count)]);
        }
    }

    // Whenever the pause menu slider is changed, change the volume on the camera and key
    public void ChangeVolume()
    {
        camSource.volume = volumeSlider.value;

        // Set a cap on max value for the sound fx to the multiplier rather than 1
        playerSource.volume = volumeSlider.value * 0.8f;
        if(roombaSource)
        {
            roombaSource.volume = volumeSlider.value * 0.6f;
        }
    }   
}
