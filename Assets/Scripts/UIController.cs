using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {

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
			// update the ui panel
			if (IsLayer(selectedObject))
			{
				// update the ui
				UpdateUI();
			}
			else
			{
				// clear the ui
				GameObject.FindGameObjectWithTag("SelectedLayerText").GetComponent<Text>().text = "";
			}
		}
	}

	/* Use raycasting to detect what object the mouse is hovering over.
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

	/* Chage the material on the specified GameObject
	 */
	private void ApplyMaterial(Material newMaterial, GameObject obj)
	{
		if (IsLayer(obj))
		{
			Renderer rend = obj.GetComponent<Renderer>();
			Material[] mats = rend.materials;
			mats[0] = newMaterial;
			rend.materials = mats;
		}
	}

	private void UpdateUI()
	{
		MemoryLayer layer = selectedObject.transform.root.GetComponent<MemoryLayer>();

		GameObject.FindGameObjectWithTag("SelectedLayerText").GetComponent<Text>().text = layer.name;
		layer.UpdateSizeLabel();
		layer.SetSizeSliderPosition();
	}

	/* Disable the currently selected layer
	 */
	public void DisableSelectedLayer()
	{
		if (IsLayer(selectedObject))
		{
			selectedObject.transform.root.gameObject.GetComponent<MemoryLayer>().ToggleLayer();
			ApplyMaterial(disabledMaterial, selectedObject);
		}
		else
		{
			Debug.Log("UI: Atempt to disable layer but no layer selected");
		}
	}

	public void UpdateLayerSize()
	{
		Slider slider = GameObject.FindGameObjectWithTag("SizeSlider").GetComponent<Slider>();

		if(IsLayer(selectedObject))
		{
			selectedObject.transform.root.gameObject.GetComponent<MemoryLayer>().UpdateSize(slider.value);
		}
	}

	/* Called when the play/pause button is pressed, halts all of the memory elements
	 * and stops the cpu making more memory requests.
	 */
	public void TogglePlayPause ()
	{
		// find all of the memory elements and toggle their movement flag
		var elements = GameObject.FindGameObjectsWithTag("MemoryElement");
		foreach(GameObject elem in elements)
		{
			elem.GetComponent<MemoryElementController>().ToggleMovement();
		}

		// toggle the cpu running flag
		GameObject.FindGameObjectWithTag("CPU").GetComponent<CPUMemoryController>().ToggleRunning();
	}

	/* Clear all of the memory elements from the currently selected layer.
	 */
	public void ClearLayerMemory ()
	{
		if(IsLayer(selectedObject))
		{
			selectedObject.transform.root.gameObject.GetComponent<MemoryLayer>().ClearLayer();
		}
	}

	/* Returns true if the provided object is a memory layer.
	 */
	private bool IsLayer(GameObject obj)
	{
		return (obj != null && obj.transform.root.gameObject.GetComponent<MemoryLayer>() != null);
	}
}