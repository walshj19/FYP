using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PacketController : MonoBehaviour {
    public float speed;

    public MemoryLayer source;
    public MemoryLayer destination;

	// Use this for initialization
	public virtual void Start () {
        
    }

    // Update is called once per frame
    public virtual void Update () {
        if (source == null)
        {
            return;
        }
        // If the packet reaches its destination it should delete itself
        if (Vector3.Distance(transform.position, destination.transform.position) < 1)
        {
            FulfillRequest();
            // then the packet can be destroyed
            Destroy(this.gameObject);
        }
        else
        {
            // Move towards its destination
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);
        }
    }

    protected abstract void FulfillRequest();
}