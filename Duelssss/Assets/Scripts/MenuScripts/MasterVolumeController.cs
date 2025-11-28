using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;

    void Start()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float sliderValue)
    {
        // Convert 0–1 slider to -80 dB to 0 dB
        float dB = Mathf.Log10(sliderValue <= 0 ? 0.0001f : sliderValue) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
    }
}