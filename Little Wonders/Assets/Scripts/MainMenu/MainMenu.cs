using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(0); // SceneManager.GetActiveScene().buildIndex
    }   
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
