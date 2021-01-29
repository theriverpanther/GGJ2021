using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Fields
    public GameObject[] triggerObjects;
    private RoomTrigger[] triggerScripts;
    private Collider[] triggerColliders;
    public GameObject key;
    public float screenTime;
    private float timer = 0;
    private bool textOnScreen = false;
    public GameObject textObject;
    private Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        triggerScripts = new RoomTrigger[triggerObjects.Length];
        triggerColliders = new Collider[triggerObjects.Length];

        // fills the triggerScripts and triggerColliders lists with all the RoomTrigger components
        for (int i = 0; i < triggerObjects.Length; i++)
        {
            triggerScripts[i] = triggerObjects[i].GetComponent<RoomTrigger>();
            triggerColliders[i] = triggerObjects[i].GetComponent<Collider>();
        }

        textComponent = textObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < triggerObjects.Length; i++)
        {
            if (triggerColliders[i].bounds.Contains(key.transform.position) && triggerScripts[i].visited == false)
            {
                timer = 0;
                DisplayText(triggerScripts[i].displayText);
                triggerScripts[i].visited = true;
                textOnScreen = true;
            }
        }

        if (textOnScreen)
        {
            timer += Time.deltaTime;
        }

        if (timer > screenTime)
        {
            ClearText();
            textOnScreen = false;
        }
    }

    // Helper Methods
    public void DisplayText(string text)
    {
        textComponent.text = text;
    }

    public void ClearText()
    {
        textComponent.text = "";
    }
}
