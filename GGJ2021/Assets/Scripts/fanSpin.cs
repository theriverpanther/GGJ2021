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
        rigidbody = gameObject.GetComponent<Rigidbody>();

        eulerRotation = new Vector3(0, rotationSpeed, 0);
    }

    void FixedUpdate()
    {
        if (spinning)
        {
            Quaternion deltaRotation = Quaternion.Euler(eulerRotation * Time.deltaTime);

            rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        }
    }

    public override void OnActivate()
    {
        base.OnActivate();

        spinning = true;
    }
}
