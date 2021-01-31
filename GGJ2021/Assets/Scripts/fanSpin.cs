using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanSpin : Activated
{
    [SerializeField]
    bool spinning = false;

    [SerializeField]
    float rotationSpeed = 10f;
    Vector3 eulerRotation;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the rigidbody variable and set the rotation
        rigidbody = gameObject.GetComponent<Rigidbody>();

        eulerRotation = new Vector3(0, rotationSpeed, 0);
    }

    void FixedUpdate()
    {
        // If the fan is on, rotate it based off of the eulerRotation
        if (spinning)
        {
            Quaternion deltaRotation = Quaternion.Euler(eulerRotation * Time.deltaTime);

            rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        }
    }

    // When the fan is activated, turn on the spinning
    public override void OnActivate()
    {
        base.OnActivate();

        spinning = true;
    }
}
