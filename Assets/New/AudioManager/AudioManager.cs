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
            Debug.LogError("There is already a SoundManager in the game !");
        }
    }
    #endregion
    
    
    private void Awake()
    {
        CreateSingleton();

        foreach (Sound sound in sounds)
        {
            SetUpSound(sound, sound.Owner);
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
    
    
    private void SetUpSound(Sound sound, GameObject owner)
    {
        sound.Owner = owner;
        sound.Source = owner.AddComponent<AudioSource>();
        sound.Source.clip = sound.clip;
        
        sound.Source.volume = sound.volume;
        sound.Source.pitch = sound.pitch;
        sound.Source.loop = sound.loop;
        sound.Source.playOnAwake = sound.playOnAwake;
        sound.Source.spatialBlend = sound.spatialBlend;
        sound.Source.maxDistance = sound.maxDistance;
        sound.Source.minDistance = sound.minDistance;
        
        UpdateSoundVolume();
    }

    public void AddNewSound(Sound sound, GameObject owner)
    {
        SetUpSound(sound,owner);
        sounds.Add(sound);
    }

    public void PlayClipAtPoint(Sound sound)
    {
        UpdateSoundVolume();
        AudioSource.PlayClipAtPoint(sound.Source.clip,sound.Owner.transform.position,sound.Source.volume);
    }
    
    
    public void PlayClipAtPointAndDestroy(Sound sound)
    {
        PlayClipAtPoint(sound);
        DeleteSound(sound,sound.clip.length);
    }
    
    public void DeleteSound(Sound sound, float time = 0)
    {
        Destroy(sound.Source,time);
        StartCoroutine(Remove(sound, time));
        print("removed");
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
        
        sound.Source.Play();
    }
    public void Play(Sound sound)
    {
        if (sound == null || !sound.Source)
        {
            //Debug.LogWarning("Sound \""+sound.name+"\" not found !");
            return;
        }
        UpdateSoundVolume();
        sound.Source.Play();
    }
    public void Stop(Sound sound)
    {
        UpdateSoundVolume();
    }
    
    public void Play(Sound sound, float delay)
    {
        StartCoroutine(Wait(sound, delay));
    }

    private IEnumerator Wait(Sound sound, float delay)
    {
        yield return new WaitForSeconds(delay);
        Play(sound);
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
            if (sound.Source)
            {
                switch(sound.type)
                {
                    case SoundType.Effects :
                        sound.Source.volume = sound.volume * volumeEffects * volumeGeneral;
                        break;
            
            
                    case SoundType.BackgroundMusic :
                        sound.Source.volume =sound.volume*volumeMusic*volumeGeneral;
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
            sound.Source.Stop();
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
            sound.Source.Stop();
        }
    }


    public void FadeOutAllSound()
    {

        foreach (Sound sound in sounds)
        {
            
            StartCoroutine(FadeOutSoundCoroutine(sound));
            DeleteSound(sound,sound.fadingTime);
        }
    }
    
    public void FadeInAllSound()
    {
        foreach (Sound sound in sounds)
        {
            StartCoroutine(FadeInSoundCoroutine(sound));
        }
    }
    public IEnumerator FadeInSoundCoroutine(Sound sound)
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

    public void FadeInSound(Sound sound)
    {
        if(!sound.Source.isPlaying)
            Play(sound);
        StartCoroutine(FadeInSoundCoroutine(sound));
    }
    public IEnumerator FadeOutSoundCoroutine(Sound sound)
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

    public void FadeOutSound(Sound sound)
    {
        StartCoroutine(FadeOutSoundCoroutine(sound));
    }
    private void SavePrefs()
    {
        PlayerPrefs.SetFloat("volumeGeneral",volumeGeneral);
        PlayerPrefs.SetFloat("volumeMusic",volumeMusic);
        PlayerPrefs.SetFloat("volumeEffects",volumeEffects);
    }
    
}
