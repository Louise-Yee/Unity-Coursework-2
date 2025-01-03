using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        // Set volume to a default audible level
        mixer.SetFloat("volume", Mathf.Log10(0.7f) * 20); // 0 dB
    }

    // Start is called before the first frame update
    public void SetVolume(float sliderValue)
    {
        // Map slider value (0.001 to 1.0) to decibel range (-30 to 0 dB)
        float volume = Mathf.Log10(sliderValue) * 20;
        mixer.SetFloat("volume", volume);
    }
}
