using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    static SoundManager _instance = null;
    public AudioSource sfxSource;
    public AudioSource musicSource;
	// Use this for initialization
	void Start () {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
	}

    private void Update()
    {
        if (GameManager.instance.pause == true)
            musicSource.Pause();
        else if (!musicSource.isPlaying)
            musicSource.Play();
            
    }

    public void playSFX(AudioClip audioClip, bool loop = false, float volume = 1.0f)
    {
        
        sfxSource.clip = audioClip;
        sfxSource.loop = loop;
        sfxSource.volume = volume;
        sfxSource.Play();
        Debug.Log("Programmer Log: Playing SFX Clip: " + sfxSource.clip);
    }

    public void playMusic(AudioClip audioClip, bool loop = true, float volume = 1.0f)
    {
        musicSource.clip = audioClip;
        musicSource.loop = loop;
        musicSource.volume = volume;
        musicSource.Play();
        Debug.Log("Programmer Log: Playing Music Clip: " + musicSource.clip);
    }

    public static SoundManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
}
