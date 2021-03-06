using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DestroyAfterDone : MonoBehaviour
{
    public bool afterSound = false, afterParticles = false;

    private AudioSource audioSource;
    private ParticleSystem particles;

    void Start(){
        if(afterSound){
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
        if(afterParticles){
            particles = GetComponent<ParticleSystem>();
            particles.Play();
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if(afterSound && !audioSource.isPlaying){
            Destroy(this.gameObject);
        }
        if(afterParticles && !particles.isPlaying){
            Destroy(this.gameObject);
        }
    }
}
