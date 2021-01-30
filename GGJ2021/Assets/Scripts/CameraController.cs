using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    KeyCode changeCamKey = KeyCode.LeftShift;

    bool isThirdPerson = true;

    [SerializeField]
    public Transform thirdPersonCam;

    public Transform mainCamera;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    Vector3 firstPersonBackwards = Vector3.back;
    Vector3 firstPersonLeft = Vector3.left;

    Vector3 thirdPersonRelative;

    int camSwitchDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (isThirdPerson && camSwitchDelay > 0) // HERE BE DRAGONS! DO. NOT. TOUCH THIS.
        {
            if (camSwitchDelay == 1)
            {
                Vector3 target = transform.position - thirdPersonCam.position;

                target.y = 0;
                firstPersonBackwards.y = 0;

                float cos = Vector3.Dot(target, firstPersonBackwards) / (Vector3.Magnitude(target) * Vector3.Magnitude(firstPersonBackwards));

                float adjustAngle = 180 - (Mathf.Acos(cos) * Mathf.Rad2Deg);

                CinemachineFreeLook thirdPersonCamMachine = thirdPersonCam.GetComponent<CinemachineFreeLook>();

                thirdPersonCamMachine.m_XAxis.Value = adjustAngle * Mathf.Sign(Vector3.Dot(target, firstPersonLeft));
            }

            camSwitchDelay--;
        }

        // Change the perspective camera in use
        if (Input.GetKeyDown(changeCamKey))
        {
            if (isThirdPerson)
            {
                Vector3 target = gameObject.transform.position - thirdPersonCam.position;
                float result = 0f;

                yRotation = Mathf.Rad2Deg * Mathf.Atan2(target.x, target.z);

                thirdPersonRelative = thirdPersonCam.position - transform.position;
            }
            else
            {
                firstPersonBackwards = -mainCamera.forward;
                firstPersonLeft = -mainCamera.right;

                camSwitchDelay = 2;
            }
            
            isThirdPerson = !isThirdPerson;

            thirdPersonCam.GetComponent<CinemachineFreeLook>().enabled = isThirdPerson;
        }
        if(!isThirdPerson)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation += mouseX;

            //Debug.Log(xRotation + "\n" + yRotation);

            mainCamera.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

            // Follow the player
            mainCamera.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f, gameObject.transform.position.z);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(thirdPersonCam.position, gameObject.transform.position);
    }
}
