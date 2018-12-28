using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {
	public bool hit;
	private int count;
    public AudioClip bumpSFX;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (hit) {
			if (count > 0) {
				transform.Translate (Vector3.up * Time.fixedDeltaTime * 2);
				count--;
			} else if (count <=0 && count > -3) {
				transform.Translate (Vector3.down * Time.fixedDeltaTime * 2);
				count--;
			} else {
				hit = false;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D c){
		if (c.collider.bounds.max.y < transform.position.y && c.collider.tag == "Player") {
			count = 3;
			hit = true;
            SoundManager.instance.playSFX(bumpSFX, false);
		}
	}
}
