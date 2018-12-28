using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScene : MonoBehaviour {
    private float t;
    public AudioClip winMusic;
	// Use this for initialization
	void Start () {
        t = 0.0f;
        playMusic(winMusic);
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if (t >= 5.5)
            loadScene("TitleScreen");
	}

    private void playMusic(AudioClip music)
    {
        SoundManager.instance.playMusic(music, false);
    }

    private void loadScene(string scene)
    {
        GameManager.instance.loadScene(scene);
    }
}
