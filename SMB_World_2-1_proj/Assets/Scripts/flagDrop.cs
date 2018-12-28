using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class flagDrop : MonoBehaviour {
    public Transform t;
    private int count;
    private bool hit;
	// Use this for initialization

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.tag == "Player")
            hit = true;

    }

    // Update is called once per frame
    void Update () {
        if (hit == true)
        {
            t.Translate(Vector3.down * Time.fixedDeltaTime * 2);
        }
    }
}
