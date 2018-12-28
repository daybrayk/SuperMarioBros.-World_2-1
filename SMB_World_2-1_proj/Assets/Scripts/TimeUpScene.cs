using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUpScene : MonoBehaviour {
    public AudioClip gameOverMusic;
    private float t;
	// Use this for initialization
	void Start () {
        t = 0;
        playMusic(gameOverMusic);
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if (t >= 4)
            loadScene("TitleScreen");
    }

    private void playMusic(AudioClip music)
    {
        SoundManager.instance.playMusic(gameOverMusic, false);
    }

    private void loadScene(string scene)
    {
        GameManager.instance.loadScene(scene);
    }
}
