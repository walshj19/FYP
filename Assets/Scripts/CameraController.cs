using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float moveSpeed = 10;
    public float scrollSpeed = 500;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        move();
    }

    // Handles moving the camera on the horizontal and vertical axis and zoom
    void move ()
    {
        // The movement scales with the time since the last frame, the set move speed and the axis magnitude
        float forward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float sideways = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        // elevation can be controlled by scrolling the mouse
        float elevation = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime * -1;

        // Apply the movement to the transform
        transform.Translate(sideways, elevation, forward);
    }
}
