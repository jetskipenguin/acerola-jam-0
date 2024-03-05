using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyDisplayController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI frequencyText;

    public void DisplayFrequency(string frequency)
    {
        frequencyText.text = frequency;
    }

    public IEnumerator DisplayFrequencyForTime(string frequency, float time)
    {
        string originalFrequency = frequencyText.text;
        frequencyText.text = frequency;
        yield return new WaitForSeconds(time);
        frequencyText.text = originalFrequency;
    }
}
