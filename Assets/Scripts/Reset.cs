using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

    public GameStates gameStates;
    public List<GameObject> buttons;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter() {
        gameStates.state = 0;
        foreach(GameObject button in buttons) {
            button.GetComponent<ResetPosition>().OnTriggerEnter(null);
        }
    }
}
