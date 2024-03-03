using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] private AbberationManagerSO abberationManager;
    [SerializeField] private FrequencyDisplayController frequencyDisplayController;
    private string[] typicalFreqs;
    private int currFreqIndex = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        abberationManager.resetMissedAbberations();

        typicalFreqs = abberationManager.getTypicalFrequencies();
    }

    public void NextFreq()
    {
        if (currFreqIndex < typicalFreqs.Length - 1)
        {
            currFreqIndex++;
            frequencyDisplayController.DisplayFrequency(typicalFreqs[currFreqIndex]);
        }
    }

    public void PrevFreq()
    {
        if (currFreqIndex > 0)
        {
            currFreqIndex--;
            frequencyDisplayController.DisplayFrequency(typicalFreqs[currFreqIndex]);
        }
    }
}
