using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip[] sounds;

    private void Awake()
    {
        instance = this;
    }

    public void PlayOneShot(AudioSource audioSource, int soundIndex)
    {
        audioSource.PlayOneShot(sounds[soundIndex]);
    }
}
