using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandeler : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in editor (SerializeField)
    [SerializeField] float levelLoadDelay = 2.0f;
    [SerializeField] AudioClip crashObstacle;
    [SerializeField] AudioClip landOnPad;

    [SerializeField] ParticleSystem crashObstacleParticles;
    [SerializeField] ParticleSystem landOnPadParticles;

    // CACHE - e.g. references for readability or speed
    AudioSource audioSource;

    // STATE - private instacne (member) variables
    bool isTransitioning = false; // we want the world to not be reacting
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C)) 
        {
            collisionDisabled = !collisionDisabled; // this will toggle collision
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled) {  return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("on friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                //SceneManager.LoadScene(1);
                break;
            //case "Fuel":
                //Debug.Log("You are getting fuel");
                //break;
            default:
                StartCrashSequence();
                break;
        }    
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(landOnPad);
        landOnPadParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashObstacle);
        crashObstacleParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay); 

    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        //SceneManager.LoadScene(0); //"Sandbox"
    }
}
