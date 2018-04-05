using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour {

    private bool is_active = false;
    public bool IsActive { get { return is_active; } }

    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    public void Activate() {
        if (!is_active) {
            is_active = true;
            OnActivate.Invoke();
        }
    }

    public void Deactivate() {
        if (is_active) {
            is_active = false;
            OnDeactivate.Invoke();
        }
    }
}
