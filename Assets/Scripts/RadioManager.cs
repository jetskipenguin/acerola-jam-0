using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] private AbberationManagerSO abberationManager;
    [SerializeField] private FrequencyDisplayController frequencyDisplayController;
    [SerializeField] private DrawWaveform drawWaveform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PauseButton pauseButton;

    private int secondsBetweenAbberations = 3;
    private int lengthOfAbberations = 5;

    
    public void Start()
    {
        // Reset values and start the first station
        abberationManager.InitializeManager();
        NextFreq();

        // Start creating abberations
        StartCoroutine(CreateAbberation());
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

    private IEnumerator CreateAbberation()
    {
        while (true) 
        {
            yield return new WaitForSeconds(secondsBetweenAbberations);
            AbberationManagerSO.AbberationType abberationType = abberationManager.GetAbberationType();

            UnityEngine.Debug.Log("Creating Abberation: " + abberationType);
            // if(abberationType == AbberationManagerSO.AbberationType.ABBERANT_FREQ)
            // {
            //     StartCoroutine(CreateFreqAbberation());
            // }
        }
    }

    private IEnumerator CreateFreqAbberation()
    {
        Tuple<int, string> originalFreq = abberationManager.AddAbberantFreqToRandomStation();
        yield return new WaitForSeconds(lengthOfAbberations);
        abberationManager.SetStationFreq(originalFreq.Item1, originalFreq.Item2);
    }
}
