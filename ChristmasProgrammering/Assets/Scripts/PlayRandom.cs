using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandom : MonoBehaviour
{
    AudioSource m_AudioSource;
    [SerializeField] AudioClip[] audios;
    AudioClip currrentClip;
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void m_PlayRandom()
    {
        currrentClip = audios[Random.Range(0,audios.Length)];
        m_AudioSource.PlayOneShot(currrentClip);
    }
}
