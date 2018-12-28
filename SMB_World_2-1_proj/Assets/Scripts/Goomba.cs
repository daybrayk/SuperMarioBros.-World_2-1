using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour {
    public float speed;
    public AudioClip deathSFX;
    Rigidbody2D rb;
    BoxCollider2D bc;
    Animator anim;
    bool isFacingLeft;
    float moveValue;
    bool _isSquish;
    public Player playerPrefab;
    // Use this for initialization
    void Start () {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
            Debug.LogWarning("Programmer Warning: Rigidbody2D not found on " + name);
        }

        if (speed <= 0)
        {
            speed = 0.75f;
            Debug.LogWarning("Programmer Warning: Speed not set on " + name + " defaulting to " + speed);
        }
        if (!bc)
            bc = GetComponent<BoxCollider2D>();
        if (!anim)
            anim = GetComponent<Animator>();
        moveValue = -1;
    }
	
	// Update is called once per frame
	void Update () {
        if(!isSquish)
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);
    }

    void flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 scaleFactor = transform.localScale;
        scaleFactor.x = -scaleFactor.x;
        transform.localScale = scaleFactor;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            playerPrefab = c.gameObject.GetComponent<Player>();
            if(playerPrefab.anim.GetBool("invincible") == true)
            {
                Vector3 scaleFactor = transform.localScale;
                scaleFactor.y = -scaleFactor.y;
                transform.localScale = scaleFactor;
                playSound(deathSFX);
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                Destroy(bc);
            }
        }   
        else if (c.gameObject.tag == "Projectile" || c.gameObject.tag == "Koopa Shell")
        {
            Vector3 scaleFactor = transform.localScale;
            scaleFactor.y = -scaleFactor.y;
            transform.localScale = scaleFactor;
            playSound(deathSFX);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            Destroy(bc);
        }
        else if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Red Mushroom" || c.gameObject.tag == "Green Mushroom")
        {
            moveValue = -moveValue;
            flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log("Programmer Log: " + c.gameObject.name + " collided with " + c.gameObject.tag);
        if (c.tag == "Enemy Barrier")
        {
            Debug.Log(c.tag);
            moveValue *= -1;
            flip();
        }
        else if (c.tag == "Player")
        {
            isSquish = true;
            playSound(deathSFX);
            anim.Play("Goomba_Squish");
        }
    }
    /*
     * Purpose: Directs all global calls to SoundManager
     */
    public void playSound(AudioClip sound)
    {
        SoundManager.instance.playSFX(sound);
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    public bool isSquish
    {
        get { return _isSquish; }
        set { _isSquish = value; }
    }
}
