using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketController : MonoBehaviour {
    public float speed = 20;

    public MemoryLayer source;
    public MemoryLayer destination;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // If the packet reaches its destination it should delete itself
        if (Vector3.Distance(transform.position, destination.transform.position) < 1)
        {
            // the distination can now reply to its request
            destination.FulfillRequest();
            // then the packet can be destroyed
            Destroy(this.gameObject);
        }

        // move in the direction of the destination
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);
    }
}
