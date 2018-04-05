using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour {

    [SerializeField]
    private GameObject target;
    public GameObject Target { get { return target; } }

    private Vector3 initial_pos = Vector3.zero;
    private Quaternion initial_rot = Quaternion.identity;
    private Vector3 anchor_pos = Vector3.zero;
    private Quaternion anchor_rot = Quaternion.identity;

    // Use this for initialization
    void Start () {
        if (target != null) {
            initial_pos = target.transform.localPosition;
            initial_rot = target.transform.localRotation;
            anchor_pos = target.transform.parent.transform.position;
            anchor_rot = target.transform.parent.transform.rotation;
        }
	}

    private void OnTriggerEnter(Collider other) {
        Debug.Log("cc");
        target.transform.localPosition = initial_pos;
        target.transform.localRotation = initial_rot;
        target.transform.parent.transform.position = anchor_pos;
        target.transform.parent.transform.rotation = anchor_rot;
    }
}
