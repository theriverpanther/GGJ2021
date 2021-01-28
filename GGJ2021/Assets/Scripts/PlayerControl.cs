using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    public GameObject playerModel;

    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    KeyCode changeCam = KeyCode.LeftShift;

    bool isThirdPerson = true;

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

    [SerializeField]
    Transform firstPersonCam;
    [SerializeField]
    Transform thirdPersonCam;

    Quaternion initRot;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<BoxCollider>();
        thirdPersonCam = gameObject.transform.GetChild(0);
        firstPersonCam = gameObject.transform.GetChild(1);
        firstPersonCam.GetComponent<CinemachineCollider>().enabled = false;
        Cursor.visible = false;
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

        if (IsGroundedV3() && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        // -- Camera Switching --

        // Set the initial rotation for the first person camera before the key bounces
        initRot = firstPersonCam.transform.rotation;

        // Change the perspective camera in use
        if(Input.GetKeyDown(changeCam))
        {
            isThirdPerson = !isThirdPerson;

            // Find the vector between both cameras
            Vector3 between = firstPersonCam.position - thirdPersonCam.position;

            // If it is transitioning to third person, make the third person cam look to the direction
            // Otherwise, make the first person camera look in the opposite direction
            if(isThirdPerson)
            {
                thirdPersonCam.rotation = Quaternion.LookRotation(between);
            }
            else
            {
                firstPersonCam.rotation = Quaternion.LookRotation(-between);
            }

            // Swap priority between cameras and enable the correct camera colliders
            thirdPersonCam.GetComponent<CinemachineFreeLook>().Priority = (isThirdPerson?10:8);
            thirdPersonCam.GetComponent<CinemachineCollider>().enabled = isThirdPerson;
            firstPersonCam.GetComponent<CinemachineFreeLook>().Priority = (!isThirdPerson?10:8);
            firstPersonCam.GetComponent<CinemachineCollider>().enabled = !isThirdPerson;            
        }
        if(isThirdPerson)
        {
            firstPersonCam.forward = thirdPersonCam.forward;
            firstPersonCam.position = thirdPersonCam.position;
            firstPersonCam.rotation = thirdPersonCam.rotation;
        }
        else
        {
            thirdPersonCam.forward = firstPersonCam.forward;
            thirdPersonCam.position = firstPersonCam.position;
            thirdPersonCam.rotation = firstPersonCam.rotation;
        }
    }

    void LateUpdate()
    {
        // Force the first person camera to rotate to it's initial position before the Update method
        if(!isThirdPerson)
        {
            firstPersonCam.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Physics.gravity * 0.1f * rigidbody.mass);

        if (holdDuration <= maxJumpHoldDuration && Input.GetKey(jumpKey))
        {
            rigidbody.AddForce(jumpDirection * holdJumpForceMag);
        }
    }

    void Jump()
    {
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

        Vector3 torque = new Vector3();

        torque.x = Random.Range(-jumpForceMag, jumpForceMag);
        torque.y = Random.Range(-2 * jumpForceMag, 2 * jumpForceMag);
        torque.z = Random.Range(-jumpForceMag, jumpForceMag);

        rigidbody.AddTorque(torque);
    }

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
}
