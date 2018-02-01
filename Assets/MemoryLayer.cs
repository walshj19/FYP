using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryLayer : MonoBehaviour
{
    public int absoluteLayerNumber;             // The absolute position of this layer in the memory heirarchy
    public GameObject packetPrefab;

    public MemoryLayer layerAbove;
    public MemoryLayer layerBelow;
    protected List<MemoryLocation> memoryLocations;

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