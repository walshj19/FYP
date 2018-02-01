using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketController : MonoBehaviour {
    public Vector3 destination;
    public float speed = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // If the packet reaches its destination it should disable itself
        if (Vector3.Distance(transform.position, destination) < 1)
        {
            this.gameObject.SetActive(false);
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
    }
}
