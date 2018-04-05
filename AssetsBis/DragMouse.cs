using UnityEngine;
using System.Collections;

public class DragMouse : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private Rigidbody rb;
    public float velocityMultiplicator = 10;
    public float maxvelocity = 100;

    void OnValidate() {
        maxvelocity = Mathf.Max(maxvelocity, 0);
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        // offset between the point of the object the mouse is pointing on and the center of the object
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    public void OnMouseDrag() {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
        Vector3 currentPosition = transform.position;
        Vector3 differenceVector = (targetPosition - currentPosition);
        Vector3 velocity = differenceVector * velocityMultiplicator * Mathf.Max(differenceVector.magnitude,1);
        if (velocity.magnitude > maxvelocity)
            velocity = velocity * maxvelocity / velocity.magnitude;
        
        rb.velocity = velocity;
    }

}
