using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumpscare1 : MonoBehaviour
{
    public GameObject zombie;
    public Animator zombieAnimator;
    public int speed;
    public AudioSource screech;
    public GameObject blackScreen;


    [SerializeField] private float _zombietime = 0.7f; //amount of time before black screen
    [SerializeField] private float _SceneChangetime = 0.5f; //amount of time before scene change

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") //player triggers jumpscare
        {
            zombieAnimator.SetBool("IsRunning", true); //Turns on Zombie's running animation
            screech.Play(); //plays screech sound
            zombie.transform.position = Vector3.MoveTowards(transform.position, new Vector3(-15.6309996f, -15.1000004f, -12.0480003f), speed * Time.deltaTime); //moves zombie towards the target
            StartCoroutine(TurnOffZombie(_zombietime)); //starts timer for blackscreen
            StartCoroutine(SwitchScene(_SceneChangetime)); //starts timer for scene change  
        }
    }

    //activates black screen
    public IEnumerator TurnOffZombie(float t)
    {
        yield return new WaitForSeconds(t);
        blackScreen.SetActive(true); 
    }

    //scene change
    public IEnumerator SwitchScene(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("Level2");

    }
}
