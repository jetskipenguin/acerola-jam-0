using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] private AbberationManagerSO abberationManager;
    [SerializeField] private FrequencyDisplayController frequencyDisplayController;
    [SerializeField] private DrawWaveform drawWaveform;
    [SerializeField] private AudioSource audioSource;

    private int currFreqIndex = 0;
    private int secondsBetweenAbberations = 3;

    
    // Start is called before the first frame update
    void Start()
    {
        abberationManager.ResetMissedAbberations();

        audioSource.clip = abberationManager.GetTypicalAudioClip(currFreqIndex);
        drawWaveform.DisplayWaveform();
        StartCoroutine(CreateAbberation());
    }


    public void NextFreq()
    {
        if (currFreqIndex < abberationManager.GetAmountOfAvailableClipsAndFreqs() - 1)
        {
            currFreqIndex++;
            audioSource.clip = abberationManager.GetTypicalAudioClip(currFreqIndex);
            frequencyDisplayController.DisplayFrequency(abberationManager.GetTypicalFreq(currFreqIndex));
            drawWaveform.DisplayWaveform();
        }
    }


    public void PrevFreq()
    {
        if (currFreqIndex > 0)
        {
            currFreqIndex--;
            audioSource.clip = abberationManager.GetTypicalAudioClip(currFreqIndex);
            frequencyDisplayController.DisplayFrequency(abberationManager.GetTypicalFreq(currFreqIndex));
            drawWaveform.DisplayWaveform();
        }
    }

    IEnumerator CreateAbberation()
    {
        while (true) 
        {
            yield return new WaitForSeconds(secondsBetweenAbberations);
            Debug.Log("Creating Abberation: " + abberationManager.GetAbberationType());
        }
    }
}
