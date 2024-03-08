using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DrawWaveform : MonoBehaviour
{
    public int width = 500;
    public int height = 100;
    public Color waveformColor = Color.green;

    public float sat = .5f;
 
    [SerializeField] private Image img;
    [SerializeField] private AudioSource audioSource;

    private Dictionary<AudioClip, Sprite> cachedWaveforms = new Dictionary<AudioClip, Sprite>();
    private AudioClip clip;

    
    // AudioSource must be changed before calling this
    public void DisplayWaveform()
    {
        clip = audioSource.clip;
        if(cachedWaveforms.ContainsKey(clip))
        {
            img.overrideSprite = cachedWaveforms[clip];
            return;
        }

        Texture2D texture = PaintWaveformSpectrum(clip, sat, width, height, waveformColor);
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
        cachedWaveforms[clip] = sprite;
        img.overrideSprite = sprite;
    }


    // TODO: this functionality could be merged into the above function by adding color as a parameter
    public void DisplayStrangeColorWaveform()
    {
        // select random color
        Color color = new Color(Random.value, Random.value, Random.value, 1.0f);

        clip = audioSource.clip;
        Texture2D texture = PaintWaveformSpectrum(clip, sat, width, height, color);
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
        img.overrideSprite = sprite;
    }


    // eventually move this away from the GUI into some other class
    public Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col) 
    {
      Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
      float[] samples = new float[audio.samples];
      float[] waveform = new float[width];
      audio.GetData(samples, 0);
      int packSize = ( audio.samples / width ) + 1;
      int s = 0;
      for (int i = 0; i < audio.samples; i += packSize) {
          waveform[s] = Mathf.Abs(samples[i]);
          s++;
      }
 
      for (int x = 0; x < width; x++) {
          for (int y = 0; y < height; y++) {
              tex.SetPixel(x, y, Color.black);
          }
      }
 
      for (int x = 0; x < waveform.Length; x++) {
          for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++) {
              tex.SetPixel(x, ( height / 2 ) + y, col);
              tex.SetPixel(x, ( height / 2 ) - y, col);
          }
      }
      tex.Apply();
 
      return tex;
  }
}