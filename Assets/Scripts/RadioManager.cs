using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Station
{
    public string freq;
    public AudioClip clip;
    private List<AbberationManager.AbberationType> abberations = new List<AbberationManager.AbberationType>();

    public static implicit operator Tuple<string, AudioClip>(Station station)
    {
        return new Tuple<string, AudioClip>(station.freq, station.clip);
    }

    public bool IsAbberationPresent(AbberationManager.AbberationType abberationType)
    {
        return abberations.Contains(abberationType);
    }

    public void AddAbberation(AbberationManager.AbberationType abberationType)
    {
        if(abberationType == AbberationManager.AbberationType.NONE)
        {
            return;
        }

        abberations.Add(abberationType);
    }

    public void RemoveAbberation(AbberationManager.AbberationType abberationType)
    {
        abberations.Remove(abberationType);
    }
}


public class RadioManager : MonoBehaviour
{
    [SerializeField] private Station[] radioStations;
    [SerializeField] private AbberationManager abberationManager;
    [SerializeField] private FrequencyDisplayController frequencyDisplayController;
    [SerializeField] private DrawWaveform drawWaveform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PauseButton pauseButton;

    private int currStationIndex = 0;

    
    public void Start()
    {
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
        if (currStationIndex < radioStations.Length - 1)
        {
            currStationIndex++;
        }
        else
        {
            currStationIndex = 0;
        }

        SwitchStation(radioStations[currStationIndex]);
    }

    public void PrevFreq()
    {
        if (currStationIndex > 0)
        {
            currStationIndex--;
        }
        else
        {
            currStationIndex = radioStations.Length - 1;
        }

        SwitchStation(radioStations[currStationIndex]);
    }

    //TODO: refactoring radiomanager and abberation manager and and add abberationcontroller and have that report abberation status to the abberation manager
    // those abberationcontrollers are always active, each represents when a place a user can click to eliminate an abberation, each reports whether or not the user found the abberation and  how many are currently present. should be generic and reused for all abberations.
    public void SetStationFreq(int index, string freq)
    {
        radioStations[index].freq = freq;
    }

    public int GetNumRadioStations()
    {
        return radioStations.Length;
    }

    public Station GetRandomStation()
    {
        return radioStations[UnityEngine.Random.Range(0, radioStations.Length)];
    }

    public Station GetCurrentStation()
    {
        return radioStations[currStationIndex];
    }

    
    public void SwitchStation(Station station)
    {
        bool isOriginalStationPlaying = audioSource.isPlaying;

        audioSource.clip = station.clip;
        frequencyDisplayController.DisplayFrequency(station.freq);

        if(station.IsAbberationPresent(AbberationManager.AbberationType.STRANGE_WAVEFORM_COLOR))
        {
            drawWaveform.DisplayStrangeColorWaveform();
        }
        else if(station.IsAbberationPresent(AbberationManager.AbberationType.STRANGE_WAVEFORM_SHAPE))
        {
            drawWaveform.DisplayStrangeShapeWaveform();
        }
        else
        {
            drawWaveform.DisplayWaveform();
        }

        if (isOriginalStationPlaying)
        {
            audioSource.Play();
        }
        
        pauseButton.UpdateIcon();
    }
}
