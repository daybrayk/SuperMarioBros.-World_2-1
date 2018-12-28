using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour {
	public GameObject blockPrefab;
	public GameObject movingCoinPrefab;
    public AudioClip coinBoxSFX;
	private bool hit;
	private int count;

	void OnCollisionEnter2D(Collision2D c){
		if (c.collider.bounds.max.y < transform.position.y && c.collider.tag == "Player") {
			hit = true;
			count = 3;
            SoundManager.instance.playSFX(coinBoxSFX);
			movingCoinPrefab = (GameObject)Instantiate (Resources.Load ("MovingCoin"), transform.position, transform.rotation);
            GameManager.instance.score += 10;
            GameManager.instance.coins += 1;

		}
			
	}

	void Update (){
		if (hit == true){
			if (count > 0) {
				transform.Translate (Vector3.up * Time.fixedDeltaTime * 2);
				count--;
			} else if (count <= 0 && count > -3) {
				transform.Translate (Vector3.down * Time.fixedDeltaTime * 2);
				count--;
			} else {
				Destroy (gameObject);
				blockPrefab = (GameObject)Instantiate (Resources.Load("CoinBlockHit"), transform.position, transform.rotation);
				hit = false;
			}
		}
	}
}
