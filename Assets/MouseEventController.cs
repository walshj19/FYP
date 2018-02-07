using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseEventController : MonoBehaviour {

    public GameObject mouseOverObject = null;
    public Material defaultMaterial;
    public Material hoverMaterial;
    public Material selectedMaterial;

	// Use this for initialization
	void Start () {
        //Debug.Log("Mouse Event Controller Started");
	}

    // Update is called once per frame
    void Update()
    {
        GameObject newMouseOverObject = CheckForMouseOver();
        // check it there was a change from the last update
        if (newMouseOverObject != mouseOverObject)
        {
            // if the last object wasn't null revert its material
            if(mouseOverObject != null)
            {
                ApplyMaterial(defaultMaterial);
            }

            // record the new object
            mouseOverObject = newMouseOverObject;

            // if the new object isn't null set its material to the hover material
            if(mouseOverObject != null)
            {
                ApplyMaterial(hoverMaterial);
                Debug.Log("New Object hit: "+mouseOverObject.name);
            }
        }
    }

    /*
     * Use raycasting to detect what object the mouse is hovering over.
     * Assigns detected object to mouseOverObject.
     */
    private GameObject CheckForMouseOver()
    {
        //Debug.Log("test1");
        Ray ray = new Ray();
        GameObject hitObject = null;
        RaycastHit hitInfo;

        // create a ray originating at thr mouse position moving away from the camera
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Draw the ray
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
        //Debug.Log("test2");

        // Do the raycast
        if (Physics.Raycast(ray, out hitInfo))
        {
            // Get the root gameobject of the collider that was hit by the ray
            hitObject = hitInfo.transform.gameObject;
            //Debug.Log("Raycast hit object " + hitObject.name);

            //Debug.Log("Raycast hit " + castInfo.Length);
        }
        else
        {
            // The raycast didn't hit anything
            //Debug.Log("Raycast hit nothing");
        }
        return hitObject;
    }

    /*
     * Chage the material on the current mouseOverObject
     */
    private void ApplyMaterial(Material newMaterial)
    {
        Renderer rend = mouseOverObject.GetComponent<Renderer>();
        Material[] mats = rend.materials;
        mats[0] = newMaterial;
        rend.materials = mats;
    }
}
