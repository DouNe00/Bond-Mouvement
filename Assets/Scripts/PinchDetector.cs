using Leap;
using Leap.Unity.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class PinchDetector : HandPoseDetector {

    protected const float MM_TO_M = 0.001f;

    [MinValue(0)]
    [Units("meters")]
    public float activation_distance = .03f; 

    [MinValue(0)]
    [Units("meters")]
    public float deactivation_distance = .04f;

    // see HandPose region
    public bool IsPinching { get { return this.IsHolding; } }
    public bool DidStartPinch { get { return this.DidStartHold; } }
    public bool DidEndPinch { get { return this.DidRelease; } }

    private Vector3 pinch_pos;
    private Quaternion pinch_rotation;

    protected virtual void OnValidate() {
        activation_distance = Mathf.Max(0, activation_distance);
        deactivation_distance = Mathf.Max(0, deactivation_distance);
        if (deactivation_distance < activation_distance) {
            deactivation_distance = activation_distance;
        }
    }

    protected override void EnsureUpToDate() {
        if (Time.frameCount == last_update_frame) {
            return;
        }
        last_update_frame = Time.frameCount;

        did_change = false;

        Hand hand = hand_model.GetLeapHand();

        if (hand == null || !hand_model.IsTracked) {
            ChangeState(false);
            return;
        }

        distance = hand.PinchDistance * MM_TO_M;
        rotation = hand.Basis.rotation.ToQuaternion();
        position = ((hand.Fingers[0].TipPosition + hand.Fingers[1].TipPosition) * .5f).ToVector3();

        if (IsActive) {
            if (distance > deactivation_distance) {
                ChangeState(false);
            }
        }
        else {
            if (distance < activation_distance) {
                ChangeState(true);
            }
        }

        if (IsActive) {
            last_position = position;
            last_rotation = rotation;
            last_distance = distance;
            last_direction = normal;
        }
        if (controls_parents_transform) {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
