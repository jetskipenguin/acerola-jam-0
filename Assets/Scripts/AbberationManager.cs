using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbberationManager : MonoBehaviour
{
    [SerializeField] private RadioManager radioManager;
    [SerializeField] private AudioClip[] abberantSounds;
    [SerializeField] private string[] abberantFreqs;
    [SerializeField] private int lengthOfAbberation = 6;
    [SerializeField] private GameObject stuckPixel;

    private Dictionary<AbberationType, bool> abberationExists = new Dictionary<AbberationType, bool>();
    private int correctReport;
    private int incorrectReport;
    

    public enum AbberationType
    {
        NONE,
        ABBERANT_FREQ,
        STUCK_PIXEL,
        STRANGE_WAVEFORM_COLOR,
    }

    public void Start()
    {
        // initialize abberation counts
        foreach(AbberationType abberationType in Enum.GetValues(typeof(AbberationType)))
        {
            abberationExists[abberationType] = false;
        }

        // TODO: just for testing right now, remove later
        StartCoroutine(CreateStrangeWaveformColorAbberation());
    }


    // Randomly picks a type of abberation that is not already in use
    private AbberationType GetAbberationType()
    {
        List<AbberationType> availableAbberations = new List<AbberationType>();
        foreach(AbberationType abberationType in Enum.GetValues(typeof(AbberationType)))
        {
            if(!abberationExists[abberationType])
            {
                availableAbberations.Add(abberationType);
            }
        }

        if(availableAbberations.Count == 0)
        {
            return AbberationType.NONE;
        }

        return availableAbberations[UnityEngine.Random.Range(0, availableAbberations.Count)];
    }


    /* 
    picks a random station and changes the freq to an abberant freq.
    returns the original freq and index so it can be changed back.
    */
    private Tuple<int, string> AddAbberantFreqToRandomStation()
    {
        string abberantFreq = abberantFreqs[UnityEngine.Random.Range(0, abberantFreqs.Length)];

        int stationIndex = UnityEngine.Random.Range(0, radioManager.GetNumRadioStations());
        string originalFreq = radioManager.GetStationFreq(stationIndex);

        radioManager.SetStationFreq(stationIndex, abberantFreq);

        return Tuple.Create(stationIndex, originalFreq);
    }


    // Will handle randomly selecting an abberation and instantiating it
    private void CreateAbberation()
    {
        AbberationType abberationType = GetAbberationType();

        UnityEngine.Debug.Log("Creating Abberation: " + abberationType);
        if(abberationType == AbberationType.ABBERANT_FREQ)
        {
            StartCoroutine(CreateFreqAbberation());
        }
    }

    
    private IEnumerator CreateFreqAbberation()
    {
        Debug.Log("Creating FREQ abberation");
        abberationExists[AbberationType.ABBERANT_FREQ] = true;
        Tuple<int, string> originalFreq = AddAbberantFreqToRandomStation();

        yield return StartCoroutine(WaitForAbberation(AbberationType.ABBERANT_FREQ));

        Debug.Log("Removing FREQ abberation");
        radioManager.SetStationFreq(originalFreq.Item1, originalFreq.Item2);
        abberationExists[AbberationType.ABBERANT_FREQ] = false;
    }


    private IEnumerator CreateStuckPixelAbberation()
    {
        Debug.Log("Creating STUCK PIXEL abberation");
        abberationExists[AbberationType.STUCK_PIXEL] = true;
        
        // activate pixel and transform it somewhere random on the screen
        stuckPixel.SetActive(true);
        stuckPixel.transform.position = new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), 0);

        yield return StartCoroutine(WaitForAbberation(AbberationType.STUCK_PIXEL));

        Debug.Log("Removing STUCK PIXEL abberation");
        stuckPixel.SetActive(false);
        abberationExists[AbberationType.STUCK_PIXEL] = false;
    }

    
    private IEnumerator CreateStrangeWaveformColorAbberation()
    {
        Debug.Log("Creating STRANGE WAVEFORM COLOR abberation");

        abberationExists[AbberationType.STRANGE_WAVEFORM_COLOR] = true;
        Station selectedStation = radioManager.GetRandomStation();
        selectedStation.SetIsColorStrange(true);

        yield return StartCoroutine(WaitForAbberation(AbberationType.STRANGE_WAVEFORM_COLOR));

        Debug.Log("Removing STRANGE WAVEFORM COLOR abberation");

        selectedStation.SetIsColorStrange(false);    
        abberationExists[AbberationType.STRANGE_WAVEFORM_COLOR] = false;
        radioManager.SwitchStation(selectedStation);
    }


    private IEnumerator WaitForAbberation(AbberationType type)
    {
        float timer = 0;
        while (timer < lengthOfAbberation && abberationExists[type])
        {
            timer += Time.deltaTime;
            yield return null;
        }

        yield return !abberationExists[type];
    }


    // Used for keeping track of whether or not a player has found an abberation
    public void ReportAbberation(string abberationType)
    {
        // I only have to do this because unity is stupid and wont let me put an enum in the inspector 
        AbberationType type = (AbberationType)Enum.Parse(typeof(AbberationType), abberationType);
        Debug.Log("Reporting Abberation: " + type);

        if(abberationExists[type])
        {
            Debug.Log("Correct Report");
            correctReport++;
        }
        else
        {
            Debug.Log("Incorrect Report");
            incorrectReport++;
        }
        
        abberationExists[type] = false;
    }
}