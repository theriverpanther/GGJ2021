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
    float jumpForceMag = 50f;
    [SerializeField]
    float jumpForceVerticalMod = 1.5f;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Set the initial rotation for the first person camera before the key bounces
        initRot = firstPersonCam.transform.rotation;
        if (Input.GetKeyDown(jumpKey) && IsGrounded())
        {
            // Get the direction the main camera is facing
            Vector3 lookDir = Camera.main.transform.forward.normalized;

            // Flatten the y value
            lookDir.y = 0;

            Vector3 jumpForce = lookDir * jumpForceMag;
            jumpForce.y = jumpForceVerticalMod * jumpForceMag;

            // Apply the jumping force to the player.
            rigidbody.AddForce(jumpForce);

            Vector3 torque = new Vector3();

            torque.x = Random.Range(-jumpForceMag, jumpForceMag);
            torque.y = Random.Range(-2 * jumpForceMag, 2 * jumpForceMag);
            torque.z = Random.Range(-jumpForceMag, jumpForceMag);

            rigidbody.AddTorque(torque);
        }
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

        // TODO: Force the cameras to transition better / at all
    }

    void LateUpdate()
    {
        // Force the first person camera to rotate to it's initial position before the Update method
        if(!isThirdPerson)
        {
            firstPersonCam.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
    }

    bool IsGrounded()
    {
        float groundDist = collider.bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, groundDist + 0.01f);

    }
}
