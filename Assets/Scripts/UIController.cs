using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public GameObject currentlySelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //get all of the static memory elements and delete them every frame
        //GameObject[] elements = GameObject.FindGameObjectsWithTag("MemoryElement");
        //foreach (GameObject element in elements)
        //{
        //    MemoryElementController controller = element.GetComponent<MemoryElementController>();
        //    if (controller.destinationLayer == null && controller.sourceLayer == null)
        //    {
        //        Destroy(element, .1f);
        //    }
        //}
    }
}
