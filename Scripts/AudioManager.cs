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
        // Inicia a reprodu��o do primeiro �udio
        PlayTrack(currentTrackIndex);
    }

    private void Update()
    {
        // Verifica se o �udio atual terminou de tocar
        if (isStopped) { return; }
        if (!audioSource.isPlaying)
        {
            // Passa para o pr�ximo �udio
            NextTrack();
        }
    }

    public void PlayTrack(int index)
    {
        if (index >= 0 && index < soundtracks.Length)
        {
            // Define o AudioClip do AudioSource e inicia a reprodu��o
            audioSource.clip = soundtracks[index];
            audioSource.Play();
            Debug.Log(" PlayTrack");
        }
    }

    public void NextTrack()
    {
        Debug.Log(" NextTrack AUDIOSOURCE");
        // Incrementa o �ndice do �udio atual
        currentTrackIndex++;

        // Verifica se j� alcan�ou o final da lista de �udio
        if (currentTrackIndex >= soundtracks.Length)
        {
            // Reinicia a reprodu��o do primeiro �udio
            currentTrackIndex = 0;
        }

        // Toca o pr�ximo �udio
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