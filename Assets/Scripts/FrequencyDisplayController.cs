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
}
