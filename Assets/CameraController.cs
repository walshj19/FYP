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
        float forward = moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        float sideways = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.Translate(sideways, 0, forward);
    }
}
