using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles main menu animation
public class MainMenu : MonoBehaviour
{
    // fields
    public GameObject[] sequenceObjects;
    public int transitionTime;
    private float elapsedTime = 0f;
    private int sequenceIndex = 0;

    private void Start()
    {
        // Make the menu objects active
        sequenceObjects[0].SetActive(true);
    }

    private void Update()
    {
        // If it is an element that needs animation, and the time passed is enough, go to the next slide
        if (sequenceIndex < sequenceObjects.Length - 1)
        {
            if (elapsedTime >= transitionTime)
            {
                elapsedTime = 0;
                NextSlide();
            }

            elapsedTime += Time.deltaTime;
        }
    }

    public void NextSlide()
    {
        sequenceObjects[sequenceIndex].SetActive(false);
        sequenceIndex++;
        sequenceObjects[sequenceIndex].SetActive(true);
    }

    // Change the scene to the House game scene
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Close out of the application
    public void QuitGame()
    {
        Debug.Log("If this was built, the application would quit!");
        Application.Quit();
    }

}
