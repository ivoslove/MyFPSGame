using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    void Start()
    {

    }

    void Awake()
    {

    }
        public void Play(AudioClip clip)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.Play();

            StartCoroutine(RemoveSoundComponent(audioSource));
        }

        IEnumerator RemoveSoundComponent(AudioSource audioSource)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            Destroy(audioSource);
        }
    
}
