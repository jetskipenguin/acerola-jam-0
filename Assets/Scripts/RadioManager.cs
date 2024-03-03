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

    
    // Start is called before the first frame update
    void Start()
    {
        abberationManager.resetMissedAbberations();

        audioSource.clip = abberationManager.getTypicalAudioClip(currFreqIndex);
        drawWaveform.DisplayWaveform();
    }


    public void NextFreq()
    {
        if (currFreqIndex < abberationManager.getAmountOfAvailableClipsAndFreqs() - 1)
        {
            currFreqIndex++;
            audioSource.clip = abberationManager.getTypicalAudioClip(currFreqIndex);
            frequencyDisplayController.DisplayFrequency(abberationManager.getTypicalFreq(currFreqIndex));
            drawWaveform.DisplayWaveform();
        }
    }


    public void PrevFreq()
    {
        if (currFreqIndex > 0)
        {
            currFreqIndex--;
            audioSource.clip = abberationManager.getTypicalAudioClip(currFreqIndex);
            frequencyDisplayController.DisplayFrequency(abberationManager.getTypicalFreq(currFreqIndex));
            drawWaveform.DisplayWaveform();
        }
    }
}
