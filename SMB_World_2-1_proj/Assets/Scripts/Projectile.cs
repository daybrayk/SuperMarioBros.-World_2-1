using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float lifetime;
    private float speed;
    public float bounceForce;
    public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        if (!rb){
            rb = gameObject.GetComponent<Rigidbody2D>();
            Debug.LogWarning("Rigid Body not set on " + name);
        }

        if (lifetime <= 0) {
            lifetime = 2.0f;
            Debug.LogWarning("lifetime not set on " + name + " defaulting to " + lifetime);
        }

        if (bounceForce <= 0)
        {
            bounceForce = 5.0f;
            Debug.LogWarning("bounceForce not set on " + name + " defaulting to " + bounceForce);
        }
        rb.velocity = new Vector2(getSpeed(), 0);

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground")
        {
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public float getSpeed(){
        return speed;
    }

    public void setSpeed(float value){
        speed = value;
    }
}
