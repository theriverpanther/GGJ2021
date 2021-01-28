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
    [SerializeField]
    float jumpForceVerticalMod = 1.5f;

    Rigidbody rigidbody;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(jumpKey) && TheCoolerIsGrounded())
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

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log(TheCoolerIsGrounded());
        //}
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
}
