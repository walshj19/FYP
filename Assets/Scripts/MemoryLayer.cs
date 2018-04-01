﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Base class for all memory layers. Holds general state such as size and methods for memory access.
 */
public class MemoryLayer : MonoBehaviour
{
    public int absoluteLayerNumber;             // The absolute position of this layer in the memory heirarchy
    public GameObject packetPrefab;             // The prefab that will be used when generating packets
    public int layerLatency = 10;

    public int maxWidth = 10;
    public int size = 100;
    public int maxSize = 300;
    public GameObject memoryElement;
    public GameObject layerBase;

    public MemoryLayer layerAbove;
    public MemoryLayer layerBelow;
    public List<int> memoryLocations;

    public bool layerDisabled = false;
    private GameObject baseShape;

    public virtual void Awake()
    {
        memoryLocations = new List<int>();
    }

    // Use this for initialization
    public virtual void Start()
    {
        CreateBase();

        //CreateMemoryElements();
    }

    // Update is called once per frame
    void Update()
    {
        CreateMemoryElements();
    }

    /*
     * Creates the base shape for the layer based on its size
     */
    private void CreateBase()
    {
        // when first drawn the layer should draw its base shape
        baseShape = Instantiate<GameObject>(
            layerBase,
            transform.position,
            transform.rotation,
            transform);
        UpdateBase();
    }

    private void UpdateBase()
    {
        float baseX = maxWidth * 3;
        float baseZ = (size / maxWidth) + 1;
        Vector3 baseScale = new Vector3(baseX, 1, baseZ);
        baseShape.transform.localScale = baseScale;
        baseShape.transform.localPosition = new Vector3(baseX / 2, 1, -(baseZ / 2));
    }

    /*
     * Creates all of the memory elements for this layer based on its size
     */
    private void CreateMemoryElements()
    {
        // add memory blocks based on size
        // so if this is ram and the size is 1024 it should add 1024 memory blocks and arrange them appropriately
        // memory blocks should be layed out in a fixed width grid 
        foreach (int address in memoryLocations)
        {
            int listPosition = memoryLocations.BinarySearch(address);
            // calculate where the element should be placed
            Vector3 pos = transform.position;
            pos.z -= (float)System.Math.Floor((double)listPosition / maxWidth);
            pos.x += (listPosition % maxWidth) * 3;
            pos.y = 1;

            //place one element
            GameObject elem = Instantiate<GameObject>(memoryElement, pos, transform.rotation, transform);
            // calculate current element number and set address field
            elem.transform.Find("Address Label").gameObject.GetComponent<TextMesh>().text = address.ToString();
        }
    }

    // Causes the cpu to ask the next memory layer for a chunk of memory
    public virtual void MakeRequest(int address)
    {
        // Check this layer for the memory address
        if (memoryLocations.BinarySearch(address) >= 0)
        {
            FulfillRequest(address);
        }
        else
        {
            // animate the packet after this layers latency
            layerAbove.MakeRequest(address);
        }
    }

    public virtual void FulfillRequest(int address)
    {
        AnimateRequest(address);

        // if the address isn't in this layers cache yet add it
        if (memoryLocations.BinarySearch(address) < 0)
        {
            memoryLocations.Add(address);
            //create the address element
            MemoryElementController packet = Instantiate<GameObject>(packetPrefab).GetComponent<MemoryElementController>();
            packet.transform.SetPositionAndRotation(GetPositionOfElement(address), transform.rotation);
            packet.Address = address;
        }
    }

    /*
     * Method to animate a packet from the above layer to this layer
     */
    public void AnimateRequest(int address)
    {
        // Animate a packet going between the two layers

        // Create a new packet object at the location of the above memory layer
        MemoryElementController packet = Instantiate<GameObject>(packetPrefab).GetComponent<MemoryElementController>();
        packet.transform.SetPositionAndRotation(transform.position, transform.rotation);
        packet.Address = address;

        // set the packet source, destination and speed
        packet.sourceLayer = this;
        packet.destinationLayer = layerBelow;
        packet.sourcePoint = GetPositionOfElement(address);
        packet.transform.position = packet.sourcePoint;
        packet.destinationPoint = layerBelow.GetPositionOfElement(address);
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

    // get the vector location of a specific memory address in this layer
    public virtual Vector3 GetPositionOfElement(int address)
    {
        Vector3 pos = new Vector3();
        pos = transform.position;
        pos.y = 2;
        pos.z -= (float)System.Math.Floor((double)address / maxWidth);
        pos.x += address % maxWidth * 3;

        return pos;
    }

    public void UpdateSize()
    {
        Slider slider = GameObject.FindGameObjectWithTag("SizeSlider").GetComponent<Slider>();

        size = (int)(slider.value * (float)maxSize);
        //Debug.Log(slider.value+" "+maxSize+" "+size);

        GameObject.FindGameObjectWithTag("Layer Size Value").GetComponent<Text>().text = size.ToString();

        UpdateBase();

        UpdateMemoryLocations();
    }

    private void UpdateMemoryLocations()
    {
        //if the list is longer than the length shrink it
        if (memoryLocations.Count > size)
        {
            //discard extra elements
            memoryLocations.RemoveRange(size, memoryLocations.Count);
        }
    }
}