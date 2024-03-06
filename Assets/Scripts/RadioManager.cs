using System;
using System.Collections;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] private AbberationManager abberationManager;
    [SerializeField] private FrequencyDisplayController frequencyDisplayController;
    [SerializeField] private DrawWaveform drawWaveform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PauseButton pauseButton;


    [SerializeField] private string[] abberantFreqs;

    private int secondsBetweenAbberations = 3;
    private int lengthOfAbberations = 5;

    
    public void Start()
    {
        // Reset values and start the first station
        abberationManager.InitializeManager();
        NextFreq();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextFreq();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevFreq();
        }
    }


    public void NextFreq()
    {
        SwitchStation(abberationManager.GetNextStation());
    }

    public void PrevFreq()
    {
        SwitchStation(abberationManager.GetPrevStation());
    }

    private void SwitchStation(Tuple<string, AudioClip> station)
    {
        bool isOriginalStationPlaying = audioSource.isPlaying;

        audioSource.clip = station.Item2;
        frequencyDisplayController.DisplayFrequency(station.Item1);
        drawWaveform.DisplayWaveform();

        if (isOriginalStationPlaying)
        {
            audioSource.Play();
        }
        
        pauseButton.UpdateIcon();
    }
}
