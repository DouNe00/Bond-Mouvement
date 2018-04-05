using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positionning : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }
}
