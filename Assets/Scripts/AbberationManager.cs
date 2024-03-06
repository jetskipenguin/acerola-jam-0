using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Station
{
    public string freq;
    public AudioClip clip;

    public static implicit operator Tuple<string, AudioClip>(Station station)
    {
        return new Tuple<string, AudioClip>(station.freq, station.clip);
    }
}

public class AbberationManager : MonoBehaviour
{
    [SerializeField] private Station[] radioStations;
    [SerializeField] private AudioClip[] abberantSounds;
    [SerializeField] private string[] abberantFreqs;

    private int missedAbberations = 0;
    private int currStationIndex = 0;

    public enum AbberationType
    {
        ABBERANT_FREQ,
        //ABBERANT_SOUND,
        // STUCK_PIXEL,
        // STRANGE_WAVEFORM,
    }

    public void Start()
    {
        StartCoroutine(CreateFreqAbberation());
    }

    public void InitializeManager()
    {
        missedAbberations = 0;
        currStationIndex = 0;
    }

    public void IncrementMissedAbberations()
    {
        missedAbberations++;
    }

    /* Functions related to switching stations */

    public Tuple<string, AudioClip> GetNextStation()
    {
        Debug.Log("CurrStationIndex: " + currStationIndex);
        if (currStationIndex < radioStations.Length - 1)
        {
            currStationIndex++;
        }
        else
        {
            currStationIndex = 0;
        }

        return radioStations[currStationIndex];
    }


    public Tuple<string, AudioClip> GetPrevStation()
    {
        if (currStationIndex > 0)
        {
            currStationIndex--;
        }
        else
        {
            currStationIndex = radioStations.Length - 1;
        }

        return radioStations[currStationIndex];
    }


    /* Functions related to creating abberations */

    public AbberationType GetAbberationType()
    {
        return (AbberationType) UnityEngine.Random.Range(0, Enum.GetValues(typeof(AbberationType)).Length);
    }

    /* 
    picks a random station and changes the freq to an abberant freq.
    returns the original freq and index so it can be changed back.
    */
    public Tuple<int, string> AddAbberantFreqToRandomStation()
    {
        string abberantFreq = abberantFreqs[UnityEngine.Random.Range(0, abberantFreqs.Length)];

        int stationIndex = UnityEngine.Random.Range(0, radioStations.Length);
        string originalFreq = radioStations[stationIndex].freq;

        SetStationFreq(stationIndex, abberantFreq);

        return Tuple.Create(stationIndex, originalFreq);
    }


    /*
    picks a random station and changes the audio to an abberant sound.
    returns the original audio and index so it can be changed bacl.
    */
    public Tuple<int, AudioClip> AddAbberantAudioToRandomStation()
    {
        AudioClip abberantSound = abberantSounds[UnityEngine.Random.Range(0, abberantSounds.Length)];

        int stationIndex = UnityEngine.Random.Range(0, radioStations.Length);

        AudioClip originalAudio = radioStations[stationIndex].clip;
        Station randomStation = radioStations[stationIndex];

        randomStation.clip = abberantSound;

        return Tuple.Create(currStationIndex, originalAudio);
    }

    
    public void SetStationFreq(int index, string freq)
    {
        radioStations[index].freq = freq;
    }


    // private IEnumerator CreateAbberations()
    // {
    //     while (true) 
    //     {
    //         yield return new WaitForSeconds(5);
    //         AbberationType abberationType = GetAbberationType();

    //         UnityEngine.Debug.Log("Creating Abberation: " + abberationType);
    //         if(abberationType == AbberationType.ABBERANT_FREQ)
    //         {
    //             StartCoroutine(CreateFreqAbberation());
    //         }
    //     }
    // }

    private IEnumerator CreateFreqAbberation()
    {
        Tuple<int, string> originalFreq = AddAbberantFreqToRandomStation();
        yield return new WaitForSeconds(3);
        SetStationFreq(originalFreq.Item1, originalFreq.Item2);
    }
}