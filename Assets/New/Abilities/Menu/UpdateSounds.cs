using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSounds : MonoBehaviour
{
    private AudioManager audioManager;

    [SerializeField] private Slider sliderGeneralVol;
    [SerializeField] private Slider sliderMusicVol;
    [SerializeField] private Slider sliderEffectsVol;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        sliderGeneralVol.value = audioManager.volumeGeneral;
        sliderMusicVol.value = audioManager.volumeMusic;
        sliderEffectsVol.value = audioManager.volumeEffects;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateGeneralVolume(float volume)
    {
        audioManager.volumeGeneral = volume;
    }
    public void UpdateMusicVolume(float volume)
    {
        audioManager.volumeMusic = volume;
    }
    public void UpdateEffectsVolume(float volume)
    {
        audioManager.volumeEffects = volume;
    }
}