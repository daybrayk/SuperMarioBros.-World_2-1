using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCoin : MonoBehaviour {
	private float savedTime;
	// Use this for initialization
	void Start () {
		savedTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if (Time.time - savedTime <= 0.75) {
			transform.Translate (Vector3.up * Time.deltaTime * 2);
		} else if (Time.time - savedTime > 0.75 && Time.time - savedTime <= 1.5) {
			transform.Translate (Vector3.down * Time.deltaTime * 2);
		} else {
			Destroy (gameObject);
		}
	}
}
