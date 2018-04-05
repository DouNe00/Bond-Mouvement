using Leap.Unity;
using Leap.Unity.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProximityDetector : Detector {

    public ProximityEvent OnProximity;

    [Units("seconds")]
    [MinValue(0)]
    public float Period = .1f;

    public GameObject[] TargetObjects;

    [Units("meters")]
    [MinValue(0)]
    public float on_distance = 0.01f;

    [Units("meters")]
    [MinValue(0)]
    public float off_distance = 0.015f;

    public GameObject current_object = null;
    public GameObject CurrentObject { get { return current_object; } }

    private bool did_change = false;
    public bool DidChangeFromLastFrame { get { return did_change; } }

    protected virtual void OnValidate() {
        if (off_distance < on_distance) {
            off_distance = on_distance;
        }
    }

    private IEnumerator proximity_watcher;
    private void Awake() {
        proximity_watcher = ProximityWatcher();
    }

    private void OnEnable() {
        StopCoroutine(proximity_watcher);
        StartCoroutine(proximity_watcher);
    }

    private void OnDisable() {
        StopCoroutine(proximity_watcher);
        Deactivate();
    }

    private IEnumerator ProximityWatcher() {
        bool proximity_state = false;
        float on_squared, off_squared;

        while (true) {
            on_squared = on_distance * on_distance;
            off_squared = off_distance * off_distance;

//            Debug.Log("onsquared : " + on_squared + "; offsquared : " + off_squared);

            if (current_object != null) {
                if (DistanceSquared(current_object) > off_squared) {
                    current_object = null;
                    proximity_state = false;
                    did_change = true;
                } else {
                    did_change = false;
                }
            } else {
                for (int obj = 0; obj < TargetObjects.Length; obj++) {
                    GameObject target = TargetObjects[obj];
                    if (DistanceSquared(target) < on_squared) {
                        current_object = target;
                        proximity_state = true;
                        OnProximity.Invoke(current_object);
                        did_change = true;
                        break;
                        // TODO : change first match to closest obj
                    }
                }
                if (current_object == null) {
                    did_change = false;
                }
            }

//            Debug.Log(current_object);
            if(proximity_state) {
                Activate();
            } else {
                Deactivate();
            }

            yield return new WaitForSeconds(Period);
        }
    }

    private float DistanceSquared(GameObject target) {
        Collider target_collider = target.GetComponent<Collider>();
        Vector3 closest_pt;
        
        if (target_collider != null) {
            closest_pt = target_collider.ClosestPointOnBounds(transform.position);
        } else {
            closest_pt = target.transform.position;
        }
//        Debug.Log(target + " ; distance : " + (closest_pt - transform.position).sqrMagnitude);
        return (closest_pt - transform.position).sqrMagnitude;

    }

    [System.Serializable]
    public class ProximityEvent : UnityEvent<GameObject> {}
}
