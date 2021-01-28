using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject playerModel;

    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    float jumpForceMag = 50f;

    Rigidbody rigidbody;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(jumpKey) && IsGrounded())
        {
            // Get the direction the main camera is facing
            Vector3 lookDir = Camera.main.transform.forward.normalized;

            // Flatten the y value
            lookDir.y = 0;

            Vector3 jumpForce = lookDir * jumpForceMag;
            jumpForce.y = jumpForceMag;

            // Apply the jumping force to the player.
            rigidbody.AddForce(jumpForce);

            Vector3 torque = new Vector3();

            torque.x = Random.Range(-jumpForceMag, jumpForceMag);
            torque.y = Random.Range(-2 * jumpForceMag, 2 * jumpForceMag);
            torque.z = Random.Range(-jumpForceMag, jumpForceMag);

            rigidbody.AddTorque(torque);
        }
    }

    bool IsGrounded()
    {
        float groundDist = collider.bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, groundDist + 0.1f);

    }
}
