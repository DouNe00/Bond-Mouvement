using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour {

    public int state = 0;
    public string[] state_tags = new string[]{"state0","state1","state2","state3"};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string get_state_tag() { return state_tags[state]; }
}
