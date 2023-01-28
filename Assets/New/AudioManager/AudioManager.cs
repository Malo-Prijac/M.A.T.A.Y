using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0,1)]public float volumeGeneral = 1;
    [Range(0,1)]public float volumeMusic = 1;
    [Range(0,1)]public float volumeEffects = 1;

    [NonReorderable]
    public List<Sound> sounds;
    // Start is called before the first frame update
    #region Singleton
    public static AudioManager instance;
    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //gameObject = instance;
            Destroy(gameObject);
            //Debug.LogError("There is already a SoundManager in the game !");
        }
    }
    #endregion
    
    
    private void Awake()
    {
        CreateSingleton();

        foreach (Sound sound in sounds)
        {
            SetUpSound(sound);
        }

    }

    private void Start()
    {
        StartPlayerPrefs();
    }

    private void StartPlayerPrefs()
    {
        volumeGeneral = PlayerPrefs.GetFloat("volumeGeneral",1);
        volumeMusic = PlayerPrefs.GetFloat("volumeMusic",1);
        volumeEffects = PlayerPrefs.GetFloat("volumeEffects",1);
    }

    private void Update()
    {
        UpdateSoundVolume();
        SavePrefs();
    }

    public void AdjustVolume(float mult)
    {
        foreach (Sound sound in sounds)
        {
            sound.volume = sound.volume * mult;
        }
    }
    
    
    private void SetUpSound(Sound sound)
    {
        sound.source = sound.Owner.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;

        //UpdateSoundVolume();
        sound.source.volume = sound.volume;
        //UpdateSoundVolume();
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }

    public void AddNewSound(Sound sound)
    {
        SetUpSound(sound);
        sounds.Add(sound);
    }
    
    public void DeleteSound(Sound sound, float time = 0)
    {
        //AudioSource[] audioSources = sound.Owner.GetComponents<AudioSource>();
        //AudioSource audioSource = Array.Find(audioSources, audioSources => audioSources == sound.source);
        Destroy(sound.source,time);
        StartCoroutine(Remove(sound, time));
        print("removed");
        //Destroy(audioSource);
    }

    private IEnumerator Remove(Sound sound, float time)
    {
        yield return new WaitForSeconds(time);
        sounds.Remove(sound);
    }

    public void Play(string soundName)
    {
        Sound sound = sounds.Find(sound => sound.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound \""+soundName+"\" not found !");
            return;
        }
        
        sound.source.Play();
    }
    public void Play(Sound sound)
    {
        if (sound == null || !sound.source)
        {
            //Debug.LogWarning("Sound \""+sound.name+"\" not found !");
            return;
        }
        sound.source.Play();
    }

    public void PlayAndDeleteAfter(Sound sound)
    {
        UpdateSoundVolume();
        Play(sound);
        DeleteSound(sound,sound.clip.length);
    }

    private void UpdateSoundVolume()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.source)
            {
                switch(sound.type)
                {
                    case SoundType.Effects :
                        sound.source.volume = sound.volume * volumeEffects * volumeGeneral;
                        break;
            
            
                    case SoundType.BackgroundMusic :
                        sound.source.volume =sound.volume*volumeMusic*volumeGeneral;
                            break;
                
                }
            }
        }

    }

    public void ClearSounds()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
        sounds.Clear();
        foreach (AudioSource audioSource in audioSources)
        {
            Destroy(audioSource);
        }
    }
    public void StopPlayingAll ()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }


    public void FadeOutAllSound()
    {

        foreach (Sound sound in sounds)
        {
            
            StartCoroutine(FadeOutSound(sound));
            DeleteSound(sound,sound.fadingTime);
        }
    }
    
    public void FadeInAllSound()
    {
        foreach (Sound sound in sounds)
        {
            StartCoroutine(FadeInSound(sound));
        }
        

    }
    IEnumerator FadeInSound(Sound sound)
    {
        float x = 0f;
        sound.volume = 0f;
        while(x<1f)
        {
            x += Time.deltaTime / (sound.fadingTime);
            
            sound.volume = sound.FadingCurve.Evaluate(x)*sound.maxVolume;
            yield return 0;
        }
    }
    IEnumerator FadeOutSound(Sound sound)
    {
        float x = 1f;
        sound.volume = 1f;
        while(x>0f)
        {
            
            x -= Time.deltaTime / (sound.fadingTime);
            sound.volume = sound.FadingCurve.Evaluate(x)*sound.maxVolume;
            yield return 0;
        }
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetFloat("volumeGeneral",volumeGeneral);
        PlayerPrefs.SetFloat("volumeMusic",volumeMusic);
        PlayerPrefs.SetFloat("volumeEffects",volumeEffects);
    }
    
    
}
