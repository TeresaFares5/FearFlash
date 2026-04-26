using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneOver : MonoBehaviour
{
    public float _cutsceneTime; //length of cutscene

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CutSceneTime(_cutsceneTime)); //starts timer for cutscene
    }

    //switches scene when time is up
    public IEnumerator CutSceneTime(float t)
    {
        yield return new WaitForSeconds(t);
        if (SceneManager.GetActiveScene().buildIndex.Equals(6))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       


    }
}
