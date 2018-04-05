using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchRT : MonoBehaviour {

    [SerializeField]
    private PinchDetector pinch_detector_L;
    public PinchDetector PinchDetectorL {
        get {
            return pinch_detector_L;
        }
        set {
            pinch_detector_L = value;
        }
    }

    [SerializeField]
    private PinchDetector pinch_detector_R;
    public PinchDetector PinchDetectorR {
        get {
            return pinch_detector_R;
        }
        set {
            pinch_detector_R = value;
        }
    }

    [SerializeField]
    private ProximityDetector proximity_detector_L;
    public ProximityDetector ProximityDetectorL {
        get {
            return proximity_detector_L;
        }
        set {
            proximity_detector_L = value;
        }
    }

    [SerializeField]
    private ProximityDetector proximity_detector_R;
    public ProximityDetector ProximityDetectorR {
        get {
            return proximity_detector_R;
        }
        set {
            proximity_detector_R = value;
        }
    }

    private Transform anchor;

    private float default_near_clip;

    private void Start() {
        GameObject pinch_control = new GameObject("RT anchor");
        anchor = pinch_control.transform;
        anchor.transform.parent = transform.parent;
        transform.parent = anchor;
    }

    private void Update() {
        bool did_update = false;
        if (pinch_detector_L != null) {
            did_update |= pinch_detector_L.DidChangeFromLastFrame;
        }
        if (pinch_detector_R != null) {
            did_update |= pinch_detector_R.DidChangeFromLastFrame;
        }

        if (did_update) {
            transform.SetParent(null, true);
        }

        // Debug.Log(this);
        // Debug.Log(proximity_detector_R.CurrentObject);

        if (pinch_detector_L != null && pinch_detector_L.IsPinching
            && proximity_detector_L != null && proximity_detector_L.CurrentObject != null && proximity_detector_L.CurrentObject.transform.parent == this) {
            TransformAnchor(pinch_detector_L);
        } else if (pinch_detector_R != null && pinch_detector_R.IsPinching
            && proximity_detector_R != null && proximity_detector_R.CurrentObject != null && proximity_detector_R.CurrentObject.transform.parent == this) {
            TransformAnchor(pinch_detector_R);
        }

        if (did_update) {
            transform.SetParent(anchor, true);
        }

    }

    private void TransformAnchor(PinchDetector pinch_detector) {
        anchor.position = pinch_detector.Position;
        anchor.rotation = pinch_detector.Rotation;
        anchor.localScale = Vector3.one;
    }
}
