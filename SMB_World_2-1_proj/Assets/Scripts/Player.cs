using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour {
    public Animator anim;
    public Rigidbody2D rb;
	public BoxCollider2D bc;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public Transform minBounds;
    public Transform maxBounds;
    public float speed;
    public float jumpForce;
    public float xmin;
    public float xmax;
    public bool isGrounded;
    public float groundCheckRadius;
    public bool isFacingLeft;
	public bool bigMario;
    public bool fireMario;
	public bool invincible;

    //Variables for projectile Spawn
    public Transform projectileSpawn;
    public float projectileSpeed;
    public Projectile projectilePrefab;

    enum AClips {Jump, Die, Shoot, Grow, Shrink, FlagPole, BigJump};
    public AudioClip[] sfxClips;
    public AudioClip coin;
    public AudioClip invincibility;
    public AudioClip themeMusic;
    // Use this for initialization
    void Start () {
        if (!anim)
        {
            anim = GetComponent<Animator>();
            Debug.LogError("Programmer Warning: Animator not found on " + name);
        }

		if (!bc) {
			bc = GetComponent<BoxCollider2D> ();
			Debug.LogWarning ("Programmer Warning: Box Collider not set in inspector on " + name);
		}
        bc.enabled = true;

        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
            Debug.LogWarning("Programmer Warning: RigidBody2D not set in inspector on " + name);
        }

		if(speed <= 0)
        {
            speed = 5.0f;
            Debug.LogWarning("Programmer Warning: Speed not set on " + name + " defaulting to " + speed);
        }

        if(jumpForce <= 0)
        {
            jumpForce = 7.0f;
            Debug.LogWarning("Programmer Warning: jumpForce not set in inspector on " + name + " defaulting to " + jumpForce);
        }

        //Set groundCheckRadius if not set in inspector
        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
            Debug.LogWarning("Programmer Warning: groundCheckRadius not set on " + name + ", defaulting to " + groundCheckRadius);
        }

        if (!groundCheck)
        {
            Debug.LogWarning("Programmer Warning: groundCheck not found on " + name);
        }

        if(projectileSpeed <= 0){
            projectileSpeed = 7.0f;
            Debug.LogWarning("Programmer Warning: projectileSpeed not set on " + name + " defaulting to " + projectileSpeed);
        }

        minBounds = GameObject.Find("MarioXMin").GetComponent<Transform>();
        maxBounds = GameObject.Find("CameraMaxBounds").GetComponent<Transform>();
        if (minBounds)
            xmin = minBounds.position.x;
        else
            Debug.LogWarning("Programmer Warning: minBounds Transform not found on object " + name);
        if (maxBounds)
            xmax = maxBounds.position.x;
        else
            Debug.LogWarning("Programmer Warning: maxBounds Transform not found on object " + name);
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.pause == false)
        {
            float moveValue = Input.GetAxis("Horizontal");
            anim.SetFloat("speed", Mathf.Abs(moveValue));
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
            xmin = minBounds.position.x;
            xmax = maxBounds.position.x;

            //Set new vector for character movement
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("noMovement"))
            {
                rb.velocity = new Vector2(0, 0);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsTag("flagPoleMovement"))
            {
                rb.velocity = new Vector2(0, -2);

            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsTag("deathMovement"))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);
            }
            //stops Mario at the left border of the camera so he cannot move backwards
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xmin, xmax), transform.position.y, transform.position.z);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //reset movement vector and apply jump force
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                //play jump audio
                if (bigMario)
                    playAudio(AClips.BigJump);
                else
                    playAudio(AClips.Jump);
            }
            //if mario isn't touching the ground don't allow him to jump
            if (!isGrounded)
            {
                //boolean value on mario's animator that checks whether he is currently jumping
                anim.SetBool("jump", true);
            }
            //flip his current animation over the y-axis, also reverses projectile direction
            if ((moveValue < 0 && !isFacingLeft) || (moveValue > 0 && isFacingLeft))
            {
                Debug.Log("Flip!");
                flip();
            }

            if (Input.GetButtonDown("Fire1") && fireMario)
            {
                fire();
            }
        }
    }

    void fire(){
        Projectile temp = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        playAudio(AClips.Shoot);
        if (isFacingLeft)
            temp.setSpeed(-projectileSpeed);
        else
            temp.setSpeed(projectileSpeed);
    }

    /*
     * Purpose: Flips Mario's animation when he changes between moving left and right
     * Callers: Update(), 
     * Dynamic Memory: None
     */
    void flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 scaleFactor = transform.localScale;
        scaleFactor.x = -scaleFactor.x;
        transform.localScale = scaleFactor;
    }

    /*
     * Purpose: decrement life counter and respawn Mario if there are lives remaining, if no lives remaining go to GameOver screen
     * Callers: Mario_Death animation (end),
     * Dynamic Memory: None
    */
    private void death()
    {
        
        GameManager.instance.lives--;
        if (GameManager.instance.lives > 0)
        {
            GameManager.instance.loadScene("Level1");
        }
        else
        {
            GameManager.instance.loadScene("GameOver");
        }
    }
    public void deathNoAnimation()
    {
        anim.Play("Mario_Die");
    }

    public void deathWithAnimation()
    {
        anim.Play("Mario_Die");
        rb.velocity = Vector2.zero;
        bc.enabled = false;
        Debug.Log("Mario Dead");
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    /*
     * Purpose: Directs most global SoundManager calls for Mario
     * Callers: Update(), fire(), 
     * Dynamic Memory: None
     */
    private void playAudio(AClips audioClip)
    {
        if (audioClip == AClips.Jump)
            SoundManager.instance.playSFX(sfxClips[0]);
        else if (audioClip == AClips.Die)
            SoundManager.instance.playMusic(sfxClips[1], false);
        else if (audioClip == AClips.Shoot)
            SoundManager.instance.playSFX(sfxClips[2]);
        else if (audioClip == AClips.Grow)
            SoundManager.instance.playSFX(sfxClips[3]);
        else if (audioClip == AClips.Shrink)
            SoundManager.instance.playSFX(sfxClips[4]);
        else if (audioClip == AClips.FlagPole)
            SoundManager.instance.playSFX(sfxClips[5]);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Death Box")
        {
            deathNoAnimation();
        }
        //WARNING * Global function access
        else if (c.tag == "Coin")
        {
            SoundManager.instance.playSFX(coin);
            Destroy(c.gameObject);
            GameManager.instance.score += 10;
            GameManager.instance.coins += 1;
        }

        //WARNING * Global variable access
        else if (c.tag == "Enemy" || c.tag == "Koopa Shell")
        {
            Debug.Log("Programmer Log: Koopa Box Trigger");
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            //WARNING * Global function access
            GameManager.instance.score += 100;
        }
        else if (c.name == "Spring")
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }

    /*
     * Purpose: Checks collisions to see what has been collided with and does something based on the object hit
     * Callers: UnityEngine
     * Dynamic Memory: None
     */
    private void OnCollisionEnter2D(Collision2D c)
    {
        //Print to console what has been collided with
        Debug.Log ("Programmer Log: Collision with " + c.gameObject.name);
		if(c.gameObject.tag == "Boundary")
			rb.velocity = Vector3.zero;
		
		if (c.gameObject.layer == 8 && isGrounded) 
			anim.SetBool ("jump", false);
		//WARNING * Global variable access
		if (c.gameObject.tag == "Red Mushroom") {
			bigMario = true;
            GameManager.instance.score += 200;
            playAudio(AClips.Grow);
            if (!anim.GetBool("flowerPower") && !anim.GetBool("bigMario")){
				bc.size = new Vector2 (0.5f, 1.0f);
				bc.offset = new Vector2 (0.0f, 0.5f);
				anim.SetBool ("bigMario", true);
				anim.Play ("Mario_Grow");

			}
		}

        //WARNING * Global Variable access
		else if (c.gameObject.tag == "Green Mushroom") {
			GameManager.instance.lives++;
		}

		else if (c.gameObject.tag == "Fire Flower") {
			Destroy (c.gameObject);
			bigMario = true;
            fireMario = true;
            //WARNING * Global function access
            GameManager.instance.score += 200;
            playAudio(AClips.Grow);
            anim.SetBool ("flowerPower", true);
			if (!anim.GetBool ("bigMario")) {
				anim.Play ("Mario_Flower_Grow");
				anim.SetBool ("bigMario", true);
			} else {
				anim.Play ("Fire_Mario_Idle");
			}

		}

		else if (c.gameObject.tag == "Star") {

            Destroy (c.gameObject);
			anim.SetBool ("invincible", true);
            //WARNING * Global function access
            SoundManager.instance.playMusic(invincibility);
			if (anim.GetBool ("bigMario")) {
				anim.Play ("Big_Invincible_Mario_Idle");
			} else {
				anim.Play ("Little_Invincible_Mario_Idle");
			}
            StartCoroutine("invincibilityDelay");
		}
        else if ((c.gameObject.tag == "Enemy" || c.gameObject.tag == "Koopa Shell") && anim.GetBool("invincible") != true)
        {
            //Shrink Mario if he is big
            if(bigMario || fireMario)
            {
                bigMario = fireMario = false;
                anim.SetBool("bigMario", false);
                anim.SetBool("flowerPower", false);
                playAudio(AClips.Shrink);
                anim.Play("Mario_Shrink");
            }
            //Kill Mario if he is small
            else
            {
                deathWithAnimation();
            }
        }

		if (c.gameObject.name == "flagPole") {
            playAudio(AClips.FlagPole);
			if(anim.GetBool("bigMario")){
				anim.Play("Big_Mario_Hold");
			}else{
				anim.Play("Little_Mario_Hold");
			}

		}

        if (c.gameObject.name == "flagBlock")
            //WARNING * Global function access
            SoundManager.instance.sfxSource.Stop();

        if (c.gameObject.name == "Flag")
            //WARNING * Global function access
            GameManager.instance.loadScene("WinScene");
    }

    IEnumerator invincibilityDelay()
    {
        yield return new WaitForSeconds(10.0f);
        anim.SetBool("invincible", false);
        //WARNING * Global function access
        SoundManager.instance.playMusic(themeMusic);
    }
}
