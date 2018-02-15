using System.Collections.Generic;
using UnityEngine;

public class MemoryLayer : MonoBehaviour
{
    public int absoluteLayerNumber;             // The absolute position of this layer in the memory heirarchy
    public GameObject packetPrefab;
    public int layerLatency;

    public MemoryLayer layerAbove;
    public MemoryLayer layerBelow;
    protected List<MemoryLocation> memoryLocations;

    public bool layerDisabled = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /* Service a memory request for a given address
     * If this layer has the location then it will return it
     * if not it will pass the request to the next layer
     */
    public MemoryLocation RequestMemory(int address)
    {
        // Search the list for the requested memory item
        foreach (MemoryLocation location in memoryLocations)
        {
            if (location.address == address)
            {
                return location;
            }
        }
        // If the requested location is not in the list pass the request up to the next layer
        return layerAbove.RequestMemory(address);
    }

    // Causes the cpu to ask the next memory layer for a chunk of memory
    public virtual void MakeRequest()
    {
        //Debug.Log("Making memory request");
        // Set the address of the memory being asked for

        // Ask the next memory layer for the memory
        // Get a reference to the next layer
        //return layerAbove.RequestMemory(address);

        // animate the packet after this layers latency
        layerAbove.MakeRequest();
    }

    
    public virtual void FulfillRequest()
    {
        AnimateRequest();
    }

    /*
     * Method to animate a packet from the above layer to this layer
     */
    public void AnimateRequest()
    {
        // Animate a packet going between the two layers
        //Transform origin = layerAbove.transform;
        //Transform destination = transform;

        // Create a new packet object at the location of the above memory layer
        PacketController packet = Instantiate<GameObject>(packetPrefab).GetComponent<PacketController>();
        packet.transform.SetPositionAndRotation(transform.position, transform.rotation);

        // set the packet source, destination and speed
        packet.source = this;
        packet.destination = layerBelow;
        packet.speed = layerLatency;
    }
 
    /*
     * Disable the layer so that requests pass through it
     */
    public void ToggleLayer()
    {
        if (layerDisabled)
        {
            // TODO: fix this, it won't work if two adjacent layers are disabled
            layerBelow.layerAbove = this;
            layerAbove.layerBelow = this;
        }
        else
        {
            // modify upper and lower layer to point to eachother
            layerBelow.layerAbove = layerAbove;
            layerAbove.layerBelow = layerBelow;
        }
        // toggle the flag
        layerDisabled = !layerDisabled;
    }

    // layerAbove Getter
    public MemoryLayer GetLayerAbove()
    {
        return layerAbove;
    }

    // layerBelow Getter
    public MemoryLayer GetLayerBelow()
    {
        return layerBelow;
    }

    // Add a new memory layer above this one
    public void InsertLayerAbove(MemoryLayer newLayer)
    {
        layerAbove = newLayer;
    }

    // Add a new memory layer below this one
    public void InsertLayerBelow(MemoryLayer newLayer)
    {
        layerBelow = newLayer;
    }
}