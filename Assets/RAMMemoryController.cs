using System.Collections; using System.Collections.Generic; using UnityEngine;  public class RAMMemoryController : MemoryLayer {  	// Use this for initialization 	void Start ()     { 		 	} 	 	// Update is called once per frame 	void Update ()     { 		 	}      // Causes the cpu to ask the next memory layer for a chunk of memory
    public override void MakeRequest()
    {
        //Debug.Log("Making memory request");
        // Set the address of the memory being asked for

        // Ask the next memory layer for the memory
        // Get a reference to the next layer
        //return layerAbove.RequestMemory(address);

        // animate the packet after this layers latency
        FulfillRequest();
    }      void LoadProgramFromDisk ()     {      } } 