using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class raypistolgun : MonoBehaviour
{


    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => startShoot());
        grabInteractable.deactivated.AddListener(x => stoptShoot());

    }

    public void startShoot()
    {

        particles.Play();

    }

    public void stoptShoot()
    {

        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
