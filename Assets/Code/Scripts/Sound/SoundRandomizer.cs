using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundRandomizer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> sounds;
    [SerializeField] private bool playOnStart = true;

    [SerializeField] private float minPitch = 0.85f;
    [SerializeField] private float maxPitch = 1.15f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        if (playOnStart) {
            Play();
        }
    }

    public void Play()
    {
        audioSource.clip = sounds[Random.Range(0, sounds.Count)];
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }
}