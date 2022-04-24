using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public GameObject sfxPrefab;
    public float sfxVolume = 1.0f;

    AudioSource musicPlayer;
    public AudioClip hitSound;

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
        musicPlayer = GetComponent<AudioSource>();
    }

    public void PlayHitSound(){
        PlaySound(hitSound);
    }

    public void PlaySound(AudioClip clip){
        if(clip == null){
            return;
        }
        GameObject sndObj = Instantiate(sfxPrefab, transform);
        AudioSource newSrc = sndObj.GetComponent<AudioSource>();
        newSrc.volume = sfxVolume;
        newSrc.clip = clip;
    }

    public void PlayMusic(AudioClip clip){
        if(clip == null || clip == musicPlayer.clip){
            return;
        }
        musicPlayer.clip = clip;
        musicPlayer.Stop();
        musicPlayer.Play();

    }
}
