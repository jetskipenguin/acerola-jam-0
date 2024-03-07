using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbberationManager : MonoBehaviour
{
    [SerializeField] private RadioManager radioManager;
    [SerializeField] private AudioClip[] abberantSounds;
    [SerializeField] private string[] abberantFreqs;

    private Dictionary<AbberationType, bool> abberationExists = new Dictionary<AbberationType, bool>();
    private int correctReport;
    private int incorrectReport;
    

    public enum AbberationType
    {
        NONE,
        ABBERANT_FREQ,
        //ABBERANT_SOUND,
        // STUCK_PIXEL,
        // STRANGE_WAVEFORM,
    }

    public void Start()
    {
        // initialize abberation counts
        foreach(AbberationType abberationType in Enum.GetValues(typeof(AbberationType)))
        {
            abberationExists[abberationType] = false;
        }

        // TODO: just for testing right now, remove later
        StartCoroutine(CreateFreqAbberation());
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
        yield return new WaitForSeconds(10);
        radioManager.SetStationFreq(originalFreq.Item1, originalFreq.Item2);
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