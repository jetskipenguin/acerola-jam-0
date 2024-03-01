using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private bool isPaused = false;

    public void Pause()
    {
        if (isPaused)
        {
            audioSource.Play();
            isPaused = false;
        }
        else
        {
            audioSource.Pause();
            isPaused = true;
        }
    }
}
