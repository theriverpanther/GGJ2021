using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMenu : MonoBehaviour
{
    // Change the scene to the House game scene
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Close out of the application
    public void QuitGame()
    {
        Debug.Log("If this was built, the application would quit!");
        Application.Quit();
    }
}
