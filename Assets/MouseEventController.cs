using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseEventController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Mouse Event Controller Started");
	}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("test1");
        Ray ray = new Ray();

        Debug.Log(Camera.main.name);
        try
        {
            // create a ray originating at thr mouse position moving away from the camera
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        // Draw the ray
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow, 10, false);
        //Debug.Log("test2");

        RaycastHit castInfo;

        // Do the raycast
        //RaycastHit[] castInfo = Physics.RaycastAll(ray);
        //if (castInfo.Length > 0)
        bool hit = Physics.Raycast(ray, out castInfo, 1000);
        Debug.Log(castInfo.point);
        if (hit)
        {
            // Get the root gameobject of the collider that was hit by the ray
            GameObject hitObject = castInfo.transform.gameObject;
            Debug.Log("Raycast hit object " + hitObject.name);

            //Debug.Log("Raycast hit " + castInfo.Length);
        }
        else
        {
            // The raycast didn't hit anything
            //Debug.Log("Raycast hit nothing");
        }
    }
}
