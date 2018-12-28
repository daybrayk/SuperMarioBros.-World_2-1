using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour {
    Transform target = null;
    public Transform cameraBoundMin, cameraBoundMax, dynamicXBound;
    float xmin, xmax, ymin, ymax;
	// Use this for initialization
	void Start () {
        GameObject g = GameObject.FindGameObjectWithTag("Player");

        if (!cameraBoundMin)
            Debug.LogError("cameraBoundMin has null value on " + name);
        if (!cameraBoundMax)
            Debug.LogError("cameraBoundMax has null value on " + name);

        if (g)
            target = g.GetComponent<Transform>();
        else
        {
            Debug.LogError("g variable has null value on " + name);
            return;
        }

        xmin = cameraBoundMin.position.x;
        ymin = cameraBoundMin.position.y;
        xmax = cameraBoundMax.position.x;
        ymax = cameraBoundMax.position.y;
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xmin, xmax), Mathf.Clamp(target.position.y, ymin, ymax), transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            transform.position = new Vector3(Mathf.Clamp(target.position.x, xmin, xmax), Mathf.Clamp(target.position.y, ymin, ymax), transform.position.z);
            xmin = dynamicXBound.position.x;
        }  
    }
}
