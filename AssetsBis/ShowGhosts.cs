using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGhosts : MonoBehaviour {

    public List<GameObject> ghosts;
    public string key;
    public GameStates gameStates;
    private bool showing;

    List<Renderer> ghost_renderers = new List<Renderer>();


    // Use this for initialization
    void Start () {
        foreach(GameObject ghost in ghosts) {
            Debug.Log(ghost);
            ghost_renderers.Add(ghost.GetComponent<Renderer>());
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(key)) {
            //foreach (GameObject ghost in ghosts) {
            //    //Debug.Log("yes: " + gameStates.state + " " + gameStates.state_tags[gameStates.state] + " " + ghost.tag);
            //    if(ghost.tag == gameStates.get_state_tag()) {
            //        Renderer rend = ghost_renderers[gameStates.state];
            //        rend.enabled = !showing;
            //    }
            //}
            //showing = !showing;
            foreach (Renderer rend in ghost_renderers) {
                rend.enabled = !showing;
            }
            showing = !showing;
        }
	}
}
