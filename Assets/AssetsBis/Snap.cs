using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour {

    public float threshold = 0.05f;
    public GameObject anchor;
    public Material snapping_material;
    public Material error_material;
    public GameStates gameStates;
    public float error_duration = 3f;
    public float pulse_length = 0.5f;

    private Material original_material;
    private Material base_material;
    private bool is_snapping = false;
    public bool IsSnapping { get { return is_snapping; } }
    private bool is_snapped = false;
    public bool IsSnapped { get { return is_snapped; } set { is_snapped = value; } }
    private bool error;
    private float error_start_time;

	void Start () {
        original_material = anchor.GetComponent<Renderer>().material;
        base_material = this.transform.GetChild(0).GetComponent<Renderer>().material;
    }
	
	void Update () {
        float distance;
        distance = Mathf.Abs((anchor.transform.position - transform.position).magnitude);
        //Debug.Log(distance);
        if(distance < threshold && !is_snapping) {
            anchor.GetComponent<Renderer>().material = snapping_material;
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            is_snapping = true;
            anchor.GetComponent<Renderer>().enabled = true;
        }
        if (distance > threshold && is_snapping) {
            anchor.GetComponent<Renderer>().material = original_material;
            transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            is_snapping = false;
            anchor.GetComponent<Renderer>().enabled = false;
        }

        if(error){
            float time_since_error = Time.time - error_start_time;
            if(time_since_error > error_duration) {
                error = false;
                transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                anchor.GetComponent<Renderer>().material = original_material;
                anchor.GetComponent<Renderer>().enabled = false;
            }
            else {
                anchor.GetComponent<Renderer>().enabled = Mathf.Round(Mathf.Sin(time_since_error*Mathf.PI*2/pulse_length)+1) == 1f;
            }
        }
    }

    public void OnMouseUp() {
        if (is_snapping && tag == gameStates.get_state_tag()) {
            transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            anchor.GetComponent<Renderer>().enabled = false;
            transform.position = anchor.transform.position;
            transform.rotation = anchor.transform.rotation;
            transform.GetChild(0).transform.position = anchor.transform.position;
            transform.GetChild(0).transform.rotation = anchor.transform.rotation;
            gameStates.state++;
//            GetComponent<DragMouse>().enabled = false;
//            transform.GetChild(0).GetComponent<DragChild>().enabled = false;
            Debug.Log("DONE");
            is_snapped = true;
            this.transform.GetChild(0).GetComponent<Renderer>().material = base_material;
        }
        else if (is_snapping && tag != gameStates.get_state_tag()) {
            error = true;
            error_start_time = Time.time;
            anchor.GetComponent<Renderer>().material = error_material;
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            anchor.GetComponent<Renderer>().enabled = true;
        }
    }
}
