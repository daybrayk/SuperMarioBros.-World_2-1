using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {
    public Rigidbody2D rb;
    public float speed;
    public float moveValue;
    public AudioClip mushroomSFX;
	// Use this for initialization
	void Start () {
        if (!rb)
            rb = gameObject.GetComponent<Rigidbody2D>();
        if(speed <= 0){
            speed = 0.5f;
            Debug.LogWarning("speed not set on " + name + " defaulting to 0.5f");
        }

        if(moveValue <= 0){
            moveValue = 2.0f;
            Debug.LogWarning("moveValue not set on " + name + " defaulting to 2.0f");
        }
    }
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);
	}

    private void OnCollisionEnter2D(Collision2D c){
        if(c.gameObject.tag == "Boundaries" || c.gameObject.tag == "Ground Block" || c.gameObject.tag == "Pipe" || c.gameObject.tag == "Red Mushroom" || c.gameObject.tag == "Enemy")
        {
            moveValue = -moveValue;
        }else if (c.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Green Mushroom")
                playSound(mushroomSFX);
            Destroy(gameObject);
        }
    }

    private void playSound(AudioClip sound)
    {
        SoundManager.instance.playSFX(sound);
    }
}
