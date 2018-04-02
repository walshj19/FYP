﻿using UnityEngine;

public abstract class PacketController : MonoBehaviour {
    public float speed;
    public float step;

    public MemoryLayer sourceLayer;
    public MemoryLayer destinationLayer;
    public Vector3 sourcePoint;
    public Vector3 destinationPoint;

	// Use this for initialization
	public virtual void Start () {
        step = Vector3.Distance(transform.position, destinationPoint)/speed;
    }

    // Update is called once per frame
    public virtual void Update () {
        if (sourceLayer == null)
        {
            return;
        }

        // Move towards its destination
        //float delta = step * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destinationPoint, step);

        // If the packet reaches its destination it should delete itself
        if (Vector3.Distance(transform.position, destinationPoint) < 1)
        {
            FulfillRequest();
            // then the packet can be destroyed
            Destroy(this.gameObject);
        }
    }

    protected abstract void FulfillRequest();
}