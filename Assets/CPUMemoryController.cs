using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPUMemoryController : MemoryLayer {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		// Check for new requests
	}

    // Causes the cpu to ask the next memory layer for a chunk of memory
    public void MakeMemoryRequest()
    {
        Debug.Log("Making memory request");
        // Set the address of the memory being asked for

        // Ask the next memory layer for the memory
        // Get a reference to the next layer
        //return layerAbove.RequestMemory(address);

        // Animate a packet going between the two layers
        Transform origin = layerAbove.transform;
        Transform destination = transform;

        // Create a new packet object at the location of the above memory layer
        PacketController packet = Instantiate<GameObject>(packetPrefab).GetComponent<PacketController>();
        packet.transform.SetPositionAndRotation(origin.position, origin.rotation);

        // Set the destination of the packet
        packet.destination = destination.position;
    }
}
