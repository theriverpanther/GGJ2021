using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // fields
    public GameObject[] sequenceObjects;
    public int transitionTime;
    private float elapsedTime = 0f;
    private int sequenceIndex = 0;

    private void Start()
    {
        sequenceObjects[0].SetActive(true);
    }

    private void Update()
    {
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

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("If this was built, the application would quit!");
        Application.Quit();
    }

}
