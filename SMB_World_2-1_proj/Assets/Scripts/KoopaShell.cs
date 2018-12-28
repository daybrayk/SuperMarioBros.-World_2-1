using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaShell : MonoBehaviour {
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    public bool moving;
    public float moveValue;
    public Player playerPrefab;
    public float speed;
    public AudioClip deathSFX;
    // Use this for initialization
    void Start () {
        bc = GetComponent<BoxCollider2D>();
        if (!bc)
            Debug.Log("Programmer Log: BoxCollider2D not found on " + name);
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            Debug.Log("Programmer Log: RigidBody2D not found on " + name);
        if (!playerPrefab)
        {
            playerPrefab = GameObject.Find("Player").GetComponent<Player>();
            Debug.Log("Programmer Log: playerRef not set in inspector... Finding Player");
        }
        if(speed <= 0)
        {
            speed = 6.0f;
            Debug.Log("Programmer Log: speed not set for" + name + " in inspector... Defaulting to " + speed);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(moving)
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);
	}

    /*
     * Purpose: Directs the global access of the sound manager through one function
     */
    private void playSound(AudioClip sound)
    {
        SoundManager.instance.playSFX(sound);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            playerPrefab = c.gameObject.GetComponent<Player>();
            if (playerPrefab.anim.GetBool("invincible") == true)
            {
                Vector3 scaleFactor = transform.localScale;
                scaleFactor.y = -scaleFactor.y;
                transform.localScale = scaleFactor;
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                playSound(deathSFX);
                Destroy(bc);
            }
        }
        if(c.gameObject.tag == "Projectile")
        {
            Vector3 scaleFactor = transform.localScale;
            scaleFactor.y = -scaleFactor.y;
            transform.localScale = scaleFactor;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            playSound(deathSFX);
            Destroy(bc);
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            if (moving)
            {
                moving = false;
            }
            else
            {
                moving = true;
            }
        }
        if (c.tag == "Enemy Barrier")
        {
            Debug.LogWarning("Mario Should Die");
            moveValue = -moveValue;
        }
    }
}
