using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryLayer : MonoBehaviour
{
    public int absoluteLayerNumber;             // The absolute position of this layer in the memory heirarchy

    private MemoryLayer layerAbove;
    private MemoryLayer layerBelow;
    private List<MemoryObject> memoryObjects;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Service a memory request for a given address
    public MemoryObject RequestMemory(int address)
    {
        return new MemoryObject();
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