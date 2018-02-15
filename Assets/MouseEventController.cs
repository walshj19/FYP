using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class MouseEventController : MonoBehaviour {

    public GameObject hoverObject = null;
    public GameObject selectedObject = null;

    public Material defaultMaterial;
    public Material hoverMaterial;
    public Material selectedMaterial;
    public Material disabledMaterial;

	// Use this for initialization
	void Start () {
        //Debug.Log("Mouse Event Controller Started");
	}

    // Update is called once per frame
    void Update()
    {
        // record current values to detect change
        GameObject oldHoverObject = hoverObject;
        GameObject oldSelectedObject = selectedObject;

        // check if the pointer is over the ui
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Get the current hover object
        hoverObject = GetObjectUnderMouse();

        // check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            selectedObject = hoverObject;
        }

        // if the hover object has changed and isn't the current selected object
        if (hoverObject != oldHoverObject)
        {
            if (oldHoverObject != selectedObject)
            {
                // reset the old hover object material
                ApplyMaterial(defaultMaterial, oldHoverObject);
            }
            if (hoverObject != selectedObject)
            {
                // highlight it
                ApplyMaterial(hoverMaterial, hoverObject);
            }
        }

        // if the selected object has changed
        if (selectedObject != oldSelectedObject)
        {
            // reset the old selected object material
            ApplyMaterial(defaultMaterial, oldSelectedObject);
            // highlight it
            ApplyMaterial(selectedMaterial, selectedObject);
        }
    }

    /*
     * Use raycasting to detect what object the mouse is hovering over.
     * Returns the GameObject under the mouse or null.
     */
    private GameObject GetObjectUnderMouse()
    {
        Ray ray = new Ray();
        GameObject hitObject = null;
        RaycastHit hitInfo;

        // create a ray originating at the mouse position moving away from the camera
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Draw the ray
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);

        // Do the raycast
        if (Physics.Raycast(ray, out hitInfo))
        {
            // Get the root gameobject of the collider that was hit by the ray
            hitObject = hitInfo.transform.gameObject;
            //Debug.Log("Raycast hit object " + hitObject.name);
        }

        return hitObject;
    }

    /*
     * Chage the material on the specified GameObject
     */
    private void ApplyMaterial(Material newMaterial, GameObject obj)
    {
        // check for null objects
        if (obj == null)
        {
            return;
        }
        Renderer rend = obj.GetComponent<Renderer>();
        Material[] mats = rend.materials;
        mats[0] = newMaterial;
        rend.materials = mats;
    }

    /*
     * Disable the currently selected layer
     */
    public void DisableSelectedLayer()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.root.gameObject.GetComponent<MemoryLayer>().ToggleLayer();
            ApplyMaterial(disabledMaterial, selectedObject);
        }
    }
}
