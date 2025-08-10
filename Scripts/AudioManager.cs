using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] soundtracks;
    private int currentTrackIndex = 0;
    public bool isStopped= false;

    private void Start()
    {
        // Inicia a reprodução do primeiro áudio
        PlayTrack(currentTrackIndex);
    }

    private void Update()
    {
        // Verifica se o áudio atual terminou de tocar
        if (isStopped) { return; }
        if (!audioSource.isPlaying)
        {
            // Passa para o próximo áudio
            NextTrack();
        }
    }

    public void PlayTrack(int index)
    {
        if (index >= 0 && index < soundtracks.Length)
        {
            // Define o AudioClip do AudioSource e inicia a reprodução
            audioSource.clip = soundtracks[index];
            audioSource.Play();
            Debug.Log(" PlayTrack");
        }
    }

    public void NextTrack()
    {
        Debug.Log(" NextTrack AUDIOSOURCE");
        // Incrementa o índice do áudio atual
        currentTrackIndex++;

        // Verifica se já alcançou o final da lista de áudio
        if (currentTrackIndex >= soundtracks.Length)
        {
            // Reinicia a reprodução do primeiro áudio
            currentTrackIndex = 0;
        }

        // Toca o próximo áudio
        PlayTrack(currentTrackIndex);
    }

    public void StopSound()
    {
        if(!isStopped)
        {
            isStopped = true;
            audioSource.Stop();
            Debug.Log("STOP AUDIOSOURCE");
        }
        else
        {
            isStopped = false;
            audioSource.Play();
            Debug.Log(" PlayTrack");
        }
        
    }
}