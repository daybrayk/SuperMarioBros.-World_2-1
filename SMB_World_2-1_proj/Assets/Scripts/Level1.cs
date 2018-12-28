using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour {
    public int spawnLocation;
    public AudioClip levelMusic;
    // Use this for initialization
    void Awake() {
        playMusic(levelMusic);
        if (spawnLocation != 0)
        {
            spawnLocation = 0;
        }
        GameManager.instance.playerSpawn(spawnLocation);
    }

    private void playMusic(AudioClip music)
    {
        SoundManager.instance.playMusic(music);
    }
}
