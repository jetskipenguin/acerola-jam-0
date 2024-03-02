using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject playIcon;
    [SerializeField] private GameObject stopIcon;

    private bool isPaused = false;

    public void Start()
    {
        playIcon.SetActive(false);
        stopIcon.SetActive(true);
    }

    public void Pause()
    {
        if (isPaused)
        {
            audioSource.Play();
            isPaused = false;

            // Update Icon
            playIcon.SetActive(false);
            stopIcon.SetActive(true);
        }
        else
        {
            audioSource.Pause();
            isPaused = true;

            // Update Icon
            playIcon.SetActive(true);
            stopIcon.SetActive(false);
        }
    }
}
