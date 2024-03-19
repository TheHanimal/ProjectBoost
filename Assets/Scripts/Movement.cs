using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in editor (SerializeField)
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainParticle;
    [SerializeField] ParticleSystem leftParticle;
    [SerializeField] ParticleSystem rightParticle;

    // CACHE - e.g. references for readability or speed
    Rigidbody rb;
    AudioSource audioSource;

    // STATE - private instacne (member) variables
    //bool isAlive;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }

        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateLeft();

        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateRight();
        }

        else
        {
            StopRotating();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

        if (!mainParticle.isPlaying)
        {
            mainParticle.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotateSpeed);
        if (!leftParticle.isPlaying)
        {
            leftParticle.Play();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotateSpeed);
        if (!rightParticle.isPlaying)
        {
            rightParticle.Play();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainParticle.Stop();
    }


    private void StopRotating()
    {
        rightParticle.Stop();
        leftParticle.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
