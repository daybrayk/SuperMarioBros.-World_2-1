using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameOver : MonoBehaviour {
    public AudioClip gameOverMusic;
    private float t;
	// Use this for initialization
	void Start () {
        t = 0;
        if (!gameOverMusic)
            Debug.LogWarning("Programmer Log: No Audio Clip found on gameOverMusic");
        else
            playMusic(gameOverMusic);
	}

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= 4)
            loadScene("TitleScreen");
    }

    /*
     * Purpose: Routes calls to a global object through a single source
     */
    private void playMusic(AudioClip music)
    {
        SoundManager.instance.playMusic(music);
    }

    /*
     * Purpose: Routes calls to a global object through a single source
     */
    private void loadScene(string scene)
    {
        GameManager.instance.loadScene(scene);
    }
}
