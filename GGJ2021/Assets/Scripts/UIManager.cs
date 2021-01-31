using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

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
    public GameObject pauseMenuUI;
    public GameObject roombaScreenUI;
    private Text textComponent;
    public static bool gameIsPaused = false;
    public static bool playerIsAlive = true;

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
            // If the trigger has the player and hasn't been visited before, activate it
            if (triggerColliders[i].bounds.Contains(key.transform.position) && triggerScripts[i].visited == false)
            {
                timer = 0;
                DisplayText(triggerScripts[i].displayText);
                triggerScripts[i].visited = true;
                textOnScreen = true;
            }
        }

        // If there's text on screen, add to the timer
        if (textOnScreen)
        {
            timer += Time.deltaTime;
        }

        // If the timer has gone past the allotted time, clear the text
        if (timer > screenTime)
        {
            ClearText();
            textOnScreen = false;
        }

        // If the player presses the pause key, toggle between the pause menu and active
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerIsAlive)
            {
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
            else
            {
                Resume();
            }
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

    // Resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        roombaScreenUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        key.GetComponent<CameraController>().thirdPersonCam.GetComponent<CinemachineFreeLook>().enabled = true;
        playerIsAlive = true;
    }

    // Pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        key.GetComponent<CameraController>().thirdPersonCam.GetComponent<CinemachineFreeLook>().enabled = false;
    }

    public void Death()
    {
        roombaScreenUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        key.GetComponent<CameraController>().thirdPersonCam.GetComponent<CinemachineFreeLook>().enabled = false;
        playerIsAlive = false;
    }

    // Quits the game
    public void QuitGame()
    {
        Debug.Log("If this was built, the application would quit!");
        Application.Quit();
    }

    // Go back to the main menu scene
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
