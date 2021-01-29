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
    Transform thirdPersonCam;

    public Transform mainCamera;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Change the perspective camera in use
        if(Input.GetKeyDown(changeCamKey))
        {
            if(isThirdPerson)
            {
                Vector3 target = gameObject.transform.position - thirdPersonCam.position;
                float result = 0f;
                
                yRotation = Mathf.Rad2Deg * Mathf.Atan2(target.x, target.z);
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
