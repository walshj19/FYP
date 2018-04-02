using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBlock : MonoBehaviour {
    public string type;     // Either "instruction" or "data"
    public int address;     // The starting address for this block

    private List<MemoryLocation> contents;  // The MemoryLocations contained within this block

	private void Awake()
	{
		contents = new List<MemoryLocation>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    int Length()
    {
        return contents.Count;
    }
}
