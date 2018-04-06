using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTranslation : MonoBehaviour {

    public float speed = .5f;

    public float maxX = 0f;
    public float minX = 0f;
    public float maxY = 0f;
    public float minY = 0f;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("z")) {
            transform.position = new Vector3(transform.position.x, Mathf.Min(transform.position.y + Time.deltaTime * speed, maxY), transform.position.z);
        }
        if (Input.GetKey("s")) {
            transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y - Time.deltaTime * speed, minY), transform.position.z);
        }
        if (Input.GetKey("q")) {
            transform.position = new Vector3(Mathf.Max(transform.position.x - Time.deltaTime * speed, minX), transform.position.y, transform.position.z);
        }
        if (Input.GetKey("d")) {
            transform.position = new Vector3(Mathf.Min(transform.position.x + Time.deltaTime * speed, maxX), transform.position.y, transform.position.z);
        }
    }
}
