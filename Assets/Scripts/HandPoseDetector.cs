using Leap.Unity;
using UnityEngine;

public abstract class HandPoseDetector : Detector {

    protected abstract void EnsureUpToDate();

    [SerializeField]
    protected HandModelBase hand_model;
    public HandModelBase HandModel { get { return hand_model; } set { hand_model = value; } }

    public bool controls_parents_transform = true;

    protected int last_update_frame = -1;
    protected bool did_change = false;

    protected Vector3 position;
    protected Quaternion rotation;
    protected Vector3 direction = Vector3.forward;
    protected Vector3 normal = Vector3.up;
    protected float distance;

    protected float last_hold_time = 0.0f;
    protected float last_release_time = 0.0f;
    protected Vector3 last_position = Vector3.zero;
    protected Quaternion last_rotation = Quaternion.identity;
    protected Vector3 last_direction = Vector3.forward;
    protected Vector3 last_normal = Vector3.up;
    protected float last_distance = 1.0f;

    protected virtual void Awake() {
        if (GetComponent<HandModelBase>() != null && controls_parents_transform == true) {
            Debug.LogWarning("Detector should not be control the HandModelBase's transform. Either attach it to its own transform or set ControlsTransform to false.");
        }
        if (hand_model == null) {
            hand_model = GetComponentInParent<HandModelBase>();
            if (hand_model == null) {
                Debug.LogWarning("The HandModel field of Detector was unassigned and the detector has been disabled.");
                enabled = false; // from behavior in unity
            }
        }
    }

    protected virtual void Update() {
        EnsureUpToDate();
    }

    public bool IsHolding {
        get {
            EnsureUpToDate();
            return IsActive; // see base region
        }
    }

    public bool DidChangeFromLastFrame {
        get {
            EnsureUpToDate();
            return did_change;
        }
    }

    public bool DidStartHold {
        get {
            EnsureUpToDate();
            return DidChangeFromLastFrame && IsHolding;
        }
    }

    public bool DidRelease {
        get {
            EnsureUpToDate();
            return DidChangeFromLastFrame && !IsHolding;
        }
    }

    public float LastHoldTime {
        get {
            EnsureUpToDate();
            return last_hold_time;
        }
    }

    public float LastReleaseTime {
        get {
            EnsureUpToDate();
            return last_release_time;
        }
    }

    public Vector3 Position {
        get {
            EnsureUpToDate();
            return position;
        }
    }
    public Vector3 LastActivePosition {
        get {
            return last_position;
        }
    }

    public Quaternion Rotation {
        get {
            EnsureUpToDate();
            return rotation;
        }
    }
    public Quaternion LastActiveRotation {
        get {
            return last_rotation;
        }
    }

    public Vector3 Direction { get { return direction; } }
    public Vector3 LastActiveDirection { get { return last_direction; } }

    public Vector3 Normal { get { return normal; } }
    public Vector3 LastActiveNormal { get { return last_normal; } }

    public float Distance { get { return distance; } }
    public float LastActiveDistance { get { return last_distance; } }

    protected virtual void ChangeState(bool shouldBeActive) {
        bool current_state = IsActive;
        if (shouldBeActive) {
            last_hold_time = Time.time;
            Activate();
        } else {
            last_release_time = Time.time;
            Deactivate();
        }
        if (current_state != IsActive) {
            did_change = true;
        }
    }
}
