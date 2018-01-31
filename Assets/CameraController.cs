using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float moveSpeed = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        move();
    }

    // Handles moving the camera on the horizontal and vertical axis
    void move ()
    {
        // The movement scales with the time since the last frame, the set move speed and the axis magnitude
        float forward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float sideways = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        // Apply the movement to the transform
        transform.Translate(sideways, 0, forward);
    }
}
