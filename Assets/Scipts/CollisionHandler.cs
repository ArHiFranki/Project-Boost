using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AudioSource))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 1f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip crashSFX;
    [SerializeField] [Range(0, 1)] private float crashSFXVolume = 0.5f;
    [SerializeField] private AudioClip successSFX;
    [SerializeField] [Range(0, 1)] private float successSFXVolume = 0.5f;

    [Header("Particles Effects")]
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private ParticleSystem successParticles;

    private AudioSource myAudioSource;

    private bool isTransitioning = false;
    private bool isCollisionDisabled = false;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || isCollisionDisabled) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collision with Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private void StartCrashSequence()
    {
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashSFX, crashSFXVolume);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(successSFX, successSFXVolume);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
}
