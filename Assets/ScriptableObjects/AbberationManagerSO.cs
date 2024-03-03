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

    public void incrementMissedAbberations()
    {
        missedAbberations++;
    }

    public void resetMissedAbberations()
    {
        missedAbberations = 0;
    }

    public string getTypicalFreq(int index)
    {
        return typicalFrequencies[index];
    }

    public AudioClip getTypicalAudioClip(int index)
    {
        return typicalAudioClips[index];
    }

    public int getAmountOfAvailableClipsAndFreqs()
    {
        return Math.Min(typicalFrequencies.Length, typicalAudioClips.Length);
    }
}