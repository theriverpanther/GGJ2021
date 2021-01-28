using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisTriggersThat : MonoBehaviour
{
    [SerializeField]
    Collider trigger;
    [SerializeField]
    List<Activated> activated;

    [SerializeField]
    bool disableWhenTriggered = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(Activated a in activated)
        {
            a.OnActivate();
        }

        if (disableWhenTriggered)
        {
            enabled = false;
        }
    }
}
