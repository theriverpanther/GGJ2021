using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for activated objects
public class Activated : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnActivate()
    {
        Debug.Log(gameObject.name + " activated!");
    }
}
