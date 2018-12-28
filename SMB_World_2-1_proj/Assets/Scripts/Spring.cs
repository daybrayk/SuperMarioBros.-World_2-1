using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start(){
		if (!anim) {
			anim = GetComponent<Animator> ();
			Debug.LogWarning ("Animator not set on " + name);
		}
	}

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.tag == "Player")
            anim.Play("Spring_Bounce");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
