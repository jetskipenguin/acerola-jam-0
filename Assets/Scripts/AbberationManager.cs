using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class AbberationManager : MonoBehaviour
{
    [SerializeField] private RadioManager radioManager;
    [SerializeField] private TextWriter textWriter;
    [SerializeField] private string[] abberantFreqs;
    [SerializeField] private int lengthOfAbberation = 10;
    [SerializeField] private GameObject stuckPixel;

    private int lengthBetweenAbberations = 10;

    private Dictionary<AbberationType, bool> abberationExists = new Dictionary<AbberationType, bool>();
    private int correctReport;
    private int incorrectReport;
    private int numAbberationsCreated = 0;
    

    public enum AbberationType
    {
        NONE,
        ABBERANT_FREQ,
        STUCK_PIXEL,
        STRANGE_WAVEFORM_COLOR,
        STRANGE_WAVEFORM_SHAPE
    }

    public void Start()
    {
        // initialize abberation counts
        foreach(AbberationType abberationType in Enum.GetValues(typeof(AbberationType)))
        {
            abberationExists[abberationType] = false;
        }

        // TODO: just for testing right now, remove later

        StartCoroutine(CreateNewAbberations());
    }

    public void Update()
    {
        // print incorrect and correct reports when b is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Correct Reports: " + correctReport + " |" +
                    "Incorrect Reports: " + incorrectReport + " |" +
                    "Length Between Abberations: " + lengthBetweenAbberations + " |" +
                    "Percent abberations found: " + (correctReport / numAbberationsCreated) * 100
                    );
        }
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


    // Will handle randomly selecting an abberation and instantiating it
    private IEnumerator CreateNewAbberations()
    {
        while(true)
        {
            // create abberations faster if player is doing well
            if(correctReport > 5)
            {
                lengthBetweenAbberations = 5;
            }
            else if(correctReport > 10)
            {
                lengthBetweenAbberations = 3;
            }

            AbberationType abberationType = GetAbberationType();

            UnityEngine.Debug.Log("Creating Abberation: " + abberationType);
            if(abberationType == AbberationType.ABBERANT_FREQ)
            {
                StartCoroutine(CreateFreqAbberation());
            }
            else if(abberationType == AbberationType.STUCK_PIXEL)
            {
                StartCoroutine(CreateStuckPixelAbberation());
            }
            else if(abberationType == AbberationType.STRANGE_WAVEFORM_COLOR)
            {
                StartCoroutine(CreateStrangeWaveformColorAbberation());
            }
            else if(abberationType == AbberationType.STRANGE_WAVEFORM_SHAPE)
            {
                StartCoroutine(CreateStrangeWaveformShapeAbberation());
            }
            else if(abberationType == AbberationType.NONE)
            {
                yield return null;
            }

            numAbberationsCreated++;
            yield return new WaitForSeconds(lengthBetweenAbberations);
        }
    }

    
    private IEnumerator CreateFreqAbberation()
    {
        Debug.Log("Creating FREQ abberation");
        
        Station selectedStation = radioManager.GetRandomStation();
        selectedStation.AddAbberation(AbberationType.ABBERANT_FREQ);

        string originalFreq = selectedStation.freq;
        selectedStation.freq = abberantFreqs[UnityEngine.Random.Range(0, abberantFreqs.Length)];

        yield return StartCoroutine(WaitForAbberation(AbberationType.ABBERANT_FREQ, selectedStation));

        Debug.Log("Removing FREQ abberation");

        selectedStation.freq = originalFreq;

        // refresh the station if it is the current station
        if(radioManager.GetCurrentStation() == selectedStation)
        {
            radioManager.SwitchStation(selectedStation);
        }

        selectedStation.RemoveAbberation(AbberationType.ABBERANT_FREQ);
    }


    private IEnumerator CreateStuckPixelAbberation()
    {
        // activate pixel and transform it somewhere random on the screen
        Debug.Log("Creating STUCK PIXEL abberation");
        stuckPixel.SetActive(true);
        stuckPixel.transform.position = new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), 0);

        yield return StartCoroutine(WaitForAbberation(AbberationType.STUCK_PIXEL, stuckPixel));

        Debug.Log("Removing STUCK PIXEL abberation");
        stuckPixel.SetActive(false);
    }

    
    private IEnumerator CreateStrangeWaveformColorAbberation()
    {
        Debug.Log("Creating STRANGE WAVEFORM COLOR abberation");
        Station selectedStation = radioManager.GetRandomStation();
        selectedStation.AddAbberation(AbberationType.STRANGE_WAVEFORM_COLOR);

        yield return StartCoroutine(WaitForAbberation(AbberationType.STRANGE_WAVEFORM_COLOR, selectedStation));

        Debug.Log("Removing STRANGE WAVEFORM COLOR abberation");
        selectedStation.RemoveAbberation(AbberationType.STRANGE_WAVEFORM_COLOR);

        // refresh the station if it is the current station   
        if(radioManager.GetCurrentStation() == selectedStation)
        {
            radioManager.SwitchStation(selectedStation);
        }
    }


    private IEnumerator CreateStrangeWaveformShapeAbberation()
    {
        Station selectedStation = radioManager.GetRandomStation();
        selectedStation.AddAbberation(AbberationType.STRANGE_WAVEFORM_SHAPE);
        Debug.Log("Added STRANGE WAVEFORM SHAPE abberation on station: " + selectedStation.freq);

        yield return StartCoroutine(WaitForAbberation(AbberationType.STRANGE_WAVEFORM_SHAPE, selectedStation));

        Debug.Log("Removing STRANGE WAVEFORM SHAPE abberation");
        selectedStation.RemoveAbberation(AbberationType.STRANGE_WAVEFORM_SHAPE);

        // refresh the station if it is the current station
        if(radioManager.GetCurrentStation() == selectedStation)
        {
            radioManager.SwitchStation(selectedStation);
        }
    }

    // used for abberations that appear on all stations
    private IEnumerator WaitForAbberation(AbberationType type, GameObject gameObject)
    {
        float timer = 0;
        while (timer < lengthOfAbberation && gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1);
            timer += 1;
        }

        yield return !abberationExists[type];
    }

    // used for abberations that appear on a specific station
    private IEnumerator WaitForAbberation(AbberationType type, Station station)
    {
        float timer = 0;
        while (timer < lengthOfAbberation && station.IsAbberationPresent(type))
        {
            yield return new WaitForSeconds(1);
            timer += 1;
        }

        yield return !abberationExists[type];
    }


    // Used for keeping track of whether or not a player has found an abberation
    public void ReportAbberation(string abberationType)
    {
        // I only have to do this because unity is stupid and wont let me put an enum in the inspector 
        AbberationType type = (AbberationType)Enum.Parse(typeof(AbberationType), abberationType);
        Debug.Log("Reporting Abberation: " + type + " on station: " + radioManager.GetCurrentStation().freq);

        if(radioManager.GetCurrentStation().IsAbberationPresent(type))
        {
            Debug.Log("Correct Report");
            correctReport++;
            radioManager.GetCurrentStation().RemoveAbberation(type);

            // write to game UI
            textWriter.WriteText("Correct Report..");
        }
        else if(type == AbberationType.STUCK_PIXEL && stuckPixel.activeSelf)
        {
            Debug.Log("Correct Report");
            stuckPixel.SetActive(false);
            correctReport++;
            radioManager.GetCurrentStation().RemoveAbberation(type);

            // write to game UI
            textWriter.WriteText("Correct Report..");
        }
        else
        {
            Debug.Log("Incorrect Report");
            incorrectReport++;

            // write to game UI
            textWriter.WriteText("Incorrect Report..");
        }
    }
}