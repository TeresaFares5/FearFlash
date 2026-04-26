using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsButton : MonoBehaviour
{
    //loads tutorial scene
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

   //loads prologue scene
    public void Interior()
    {
        SceneManager.LoadScene("Interior");
    }

   //loads diner scene
    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }
    
    //loads cutscene
    public void Cutscene()
    {
        SceneManager.LoadScene("Cutscene");
    }

    //quits game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

}

