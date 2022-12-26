using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

    }
    #endregion

    public Sound[] sounds;
    public AudioSource musicSource, sfxSource;


    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        musicSource.clip = s.clip;
        musicSource.Play();
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        sfxSource.PlayOneShot(s.clip);
    }

    public void ButtonClick()
    {
        PlaySFX("Button Click");
    }
}


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
