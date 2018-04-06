using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragChild : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown() {
        transform.parent.GetComponent<DragMouse>().OnMouseDown();
    }

    private void OnMouseDrag() {
        transform.parent.GetComponent<DragMouse>().OnMouseDrag();
    }

    private void OnMouseUp() {
        transform.parent.GetComponent<Snap>().OnMouseUp();
    }
}
