using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Koopa : MonoBehaviour {
	public float speed;
	Rigidbody2D rb;
    BoxCollider2D bc;
    Animator anim;
    bool isFacingLeft;
	float moveValue;
    bool _isShell;
    public Player playerPrefab;
    public AudioClip deathSFX;
    public KoopaShell koopaShellPrefab;
	// Use this for initialization
	void Start () {
		if (!rb) 
		{
			rb = GetComponent<Rigidbody2D> ();
			Debug.LogWarning ("Programmer Warning: Rigidbody2D not found on " + name);
		}

		if (speed <= 0) 
		{
			speed = 0.75f;
			Debug.LogWarning ("Programmer Warning: Speed not set on " + name + " defaulting to " + speed);
		}
        if(!bc)
            bc = GetComponent<BoxCollider2D>();
        if (!anim)
            anim = gameObject.GetComponent<Animator>();
		moveValue = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if(!isShell)
		    rb.velocity = new Vector2 (moveValue * speed, rb.velocity.y);
	}


    private void death()
    {

    }
    
    private void playAudio()
    {

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
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                playSound(deathSFX);
                Destroy(bc);
            }
        }
        else if (c.gameObject.tag == "Projectile" || c.gameObject.tag == "Koopa Shell")
        {
            Vector3 scaleFactor = transform.localScale;
            scaleFactor.y = -scaleFactor.y;
            transform.localScale = scaleFactor;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            playSound(deathSFX);
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
        if (c.tag == "Enemy Barrier")
        {
            Debug.Log("Programmer Log: " + gameObject.name + " collided with " + c.gameObject.tag);
            moveValue = -moveValue;
            flip();
        }
        else if (c.tag == "Player")
        {
            if (!isShell)
            {
                anim.Play("Koopa_Shell");
                toggleShell();
            }
            else
            {
                playSound(deathSFX);
                KoopaShell temp = Instantiate(koopaShellPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
                temp.moving = true;
            }
        }
    }

    /*
     * Purpose: Directs all global calls to SoundManager
     */
    private void playSound(AudioClip sound)
    {
        SoundManager.instance.playSFX(sound, false);
    }

    public void toggleShell()
    {
        if (isShell == true)
            isShell = false;
        else
            isShell = true;
    }

    public bool isShell
    {
        get { return _isShell; }
        set { _isShell = value; }
    }
}
