using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AbberationManager", menuName = "ScriptableObjects/AbberationManager", order = 1)]
public class AbberationManagerSO : ScriptableObject
{
    [SerializeField] private AudioClip[] typicalAudioClips;
    [SerializeField] private AudioClip[] abberantAudioClips;

    [SerializeField] private string[] typicalFrequencies = {"101.7", "102.3", "103.1", "104.5", "105.3", "106.1", "107.5", "108.3"};
    private readonly string[] abberantFrequencies = {"66.6", "666.0", "----"};

    private int missedAbberations = 0;

    public void Start()
    {
        if(typicalAudioClips.Length != typicalFrequencies.Length)
        {
            Debug.LogError("Typical Audio Clips and Frequencies do not match in length");
        }
    }

    public enum AbberationType
    {
        RADIO_STATIC,
        ABBERANT_FREQ,
        STUCK_PIXEL,
        STRANGE_WAVEFORM,
        EVIL_VOICE
    }

    public void IncrementMissedAbberations()
    {
        missedAbberations++;
    }

    public void ResetMissedAbberations()
    {
        missedAbberations = 0;
    }

    public string GetTypicalFreq(int index)
    {
        return typicalFrequencies[index];
    }

    public AudioClip GetTypicalAudioClip(int index)
    {
        return typicalAudioClips[index];
    }

    public int GetAmountOfAvailableClipsAndFreqs()
    {
        return Math.Min(typicalFrequencies.Length, typicalAudioClips.Length);
    }

    public AbberationType GetAbberationType()
    {
        return (AbberationType) UnityEngine.Random.Range(0, Enum.GetValues(typeof(AbberationType)).Length);
    }
}