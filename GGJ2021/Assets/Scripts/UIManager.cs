using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Fields
    public GameObject[] triggerObjects;
    private RoomTrigger[] triggerScripts;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < triggerObjects.Length; i++)
        {
            triggerScripts[i] = triggerObjects[i].GetComponent<RoomTrigger>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
