using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject playIcon;
    [SerializeField] private GameObject stopIcon;

    public void Start()
    {
        Pause();
    }

    public void Pause()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        if(audioSource.isPlaying)
        {
            playIcon.SetActive(false);
            stopIcon.SetActive(true);
        }
        else
        {
            playIcon.SetActive(true);
            stopIcon.SetActive(false);
        }
    }
}
