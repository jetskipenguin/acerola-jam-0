using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyDisplayController : MonoBehaviour
{
    [SerializeField] private AbberationManagerSO abberationManager;
    [SerializeField] private TMPro.TextMeshProUGUI frequencyText;

    private string[] typicalFreqs;
    private int currFreqIndex = 0;

    // Start is called before the first frame update
    public void Start()
    {
        // TODO: this is a bad place to put this but it works for now
        abberationManager.resetMissedAbberations();

        typicalFreqs = abberationManager.getTypicalFrequencies();
    }

    public void nextFreq()
    {
        if (currFreqIndex < typicalFreqs.Length - 1)
        {
            currFreqIndex++;
            frequencyText.text = typicalFreqs[currFreqIndex];
        }
    }

    public void prevFreq()
    {
        if (currFreqIndex > 0)
        {
            currFreqIndex--;
            frequencyText.text = typicalFreqs[currFreqIndex];
        }
    }
}
