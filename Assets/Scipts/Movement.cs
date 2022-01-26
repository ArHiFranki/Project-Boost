using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float mainThrust = 1000f;
    [SerializeField] private float rotationThrust = 100f;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] [Range(0, 1)] private float mainEngineVolume = 0.5f;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem leftTrusterParticles;
    [SerializeField] private ParticleSystem rightTrusterParticles;

    private Rigidbody myRigidbody;
    private AudioSource myAudioSource;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

            if (!myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(mainEngine, mainEngineVolume);
            }

            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }
        }
        else
        {
            myAudioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);

            if (!leftTrusterParticles.isPlaying)
            {
                leftTrusterParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);

            if (!rightTrusterParticles.isPlaying)
            {
                rightTrusterParticles.Play();
            }
        }
        else
        {
            leftTrusterParticles.Stop();
            rightTrusterParticles.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        myRigidbody.freezeRotation = false;
    }
}
