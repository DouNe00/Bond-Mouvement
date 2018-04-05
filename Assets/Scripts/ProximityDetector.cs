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

            did_change = false;
            GameObject closest = null;
            float min_distance = current_object == null ? on_squared : Mathf.Min(DistanceSquared(current_object), on_squared);
            for (int obj = 0; obj < TargetObjects.Length; obj++) {
                GameObject target = TargetObjects[obj];
                float distance = DistanceSquared(target);
                if (distance < min_distance) {
                    closest = target;
                    min_distance = distance;
                    proximity_state = true;
                }
            }
            if (closest == null && current_object != null) {
                if (DistanceSquared(current_object) < off_squared) {
                    closest = current_object;
                }
            }
            if (current_object != closest) did_change = true;
            current_object = closest;
            if (current_object == null) proximity_state = false;

            if (proximity_state) {
                OnProximity.Invoke(current_object);
            }

//            Debug.Log(current_object);
            if(proximity_state) {
                OnProximity.Invoke(current_object);
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
