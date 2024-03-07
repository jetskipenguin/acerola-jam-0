using System;
using System.Collections;
using UnityEngine;


public class AbberationManager : MonoBehaviour
{
    [SerializeField] private RadioManager radioManager;
    [SerializeField] private AudioClip[] abberantSounds;
    [SerializeField] private string[] abberantFreqs;

    private int missedAbberations = 0;

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


    public void IncrementMissedAbberations()
    {
        missedAbberations++;
    }

    /* Functions related to switching stations */

    


    /* Functions related to creating abberations */

    // Randomly picks a type of abberation
    private AbberationType GetAbberationType()
    {
        return (AbberationType) UnityEngine.Random.Range(0, Enum.GetValues(typeof(AbberationType)).Length);
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
        radioManager.SetStationFreq(originalFreq.Item1, originalFreq.Item2);
    }
}