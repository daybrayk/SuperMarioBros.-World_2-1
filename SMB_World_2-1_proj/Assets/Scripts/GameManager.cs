using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    static GameManager _instance = null;
    public GameObject playerPrefab;
    public AudioClip pauseSFX;
    private int _score;
    private int _lives;
    private int _clock;
    private int _coins;
    private bool _pause;
	// Use this for initialization
	void Start () {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        coins = 0;
        lives = 2;
        score = 0;
        clock = 120;
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        else if (Input.GetKeyDown(KeyCode.P))
        {
            playSound(pauseSFX);
            if (pause == true)
            {
                Time.timeScale = 1;
                pause = false;
            }
            else
            {
                Time.timeScale = 0;
                pause = true;
            }
        }
    }

    private void playSound(AudioClip sound)
    {
        SoundManager.instance.playSFX(sound);
    }

    public void playerSpawn (int spawnLocation)
    {
        string spawnPointName = SceneManager.GetActiveScene().name + "_" + spawnLocation;
        Transform spawnPointTransform = GameObject.Find(spawnPointName).GetComponent<Transform>();
        if(playerPrefab && spawnPointTransform)
        {
            Instantiate(playerPrefab, spawnPointTransform.position, spawnPointTransform.rotation);
        }
        else
        {
            Debug.LogError("Programmer Warning: Missing PlayerPrefab or spawnPointTransform!");
        }
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void quit()
    {
        Application.Quit();
    }

    /*public void startGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void gameOver()
    {
        SceneManager.LoadScene("Game_Over");
    }*/

    private void timer()
    {
        clock--;
        if (clock <= 0)
            loadScene("TimeUp");
    }

    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    
    public int score
    {
        get { return _score; }
        set { _score = value; }
    }

    public int lives
    {
        get { return _lives; }
        set { _lives = value; }
    }

    public int clock
    {
        get { return _clock; }
        set { _clock = value; }
    }

    public int coins
    {
        get { return _coins; }
        set { _coins = value; }
    }

    public bool pause
    {
        get { return _pause; }
        set { _pause = value; }
    }
}
