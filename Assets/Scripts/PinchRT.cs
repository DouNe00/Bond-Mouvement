using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchRT : MonoBehaviour {

    [SerializeField]
    private Material default_material;
    public Material DefaultMaterial {
        get {
            return default_material;
        }
        set {
            default_material = value;
        }
    }

    [SerializeField]
    private Material selected_material;
    public Material SelectedMaterial {
        get {
            return selected_material;
        }
        set {
            selected_material = value;
        }
    }

    [SerializeField]
    private Material pinched_material;
    public Material PinchedMaterial {
        get {
            return pinched_material;
        }
        set {
            pinched_material = value;
        }
    }

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

    private bool proximity_lock = false;

    private void Update() {
        bool did_update = false;
        if (pinch_detector_L != null) {
            did_update |= pinch_detector_L.DidChangeFromLastFrame;
        }
        if (pinch_detector_R != null) {
            did_update |= pinch_detector_R.DidChangeFromLastFrame;
        }

        if (proximity_detector_L != null) {
            proximity_lock = (proximity_detector_L.DidChangeFromLastFrame || proximity_lock) && !did_update; 
        }
        if (proximity_detector_R != null) {
            proximity_lock |= (proximity_detector_R.DidChangeFromLastFrame || proximity_lock) && !did_update;
        }

        if (did_update || proximity_lock) {
            transform.SetParent(null, true);
        }

        MeshRenderer renderer = this.transform.GetChild(0).GetComponent<MeshRenderer>();

        if ((proximity_detector_L.CurrentObject != null && proximity_detector_L.CurrentObject.transform.parent.name.Equals(this.name)) ||
            (proximity_detector_R.CurrentObject != null && proximity_detector_R.CurrentObject.transform.parent.name.Equals(this.name))) {
            renderer.material = selected_material;
        }
        else {
            renderer.material = default_material;
        }

        if (pinch_detector_L != null && pinch_detector_L.IsPinching
            && proximity_detector_L != null && proximity_detector_L.CurrentObject != null && proximity_detector_L.CurrentObject.transform.parent.name.Equals(this.name)) {

            TransformAnchor(pinch_detector_L);
            renderer.material = pinched_material;

        } else if (pinch_detector_R != null && pinch_detector_R.IsPinching
            && proximity_detector_R != null && proximity_detector_R.CurrentObject != null && proximity_detector_R.CurrentObject.transform.parent.name.Equals(this.name)) {

            TransformAnchor(pinch_detector_R);
            renderer.material = pinched_material;
        }

        if (did_update || proximity_lock) {
            transform.SetParent(anchor, true);
        }

    }

    private void TransformAnchor(PinchDetector pinch_detector) {
        anchor.position = pinch_detector.Position;
        anchor.rotation = pinch_detector.Rotation;
        anchor.localScale = Vector3.one;
    }
}
