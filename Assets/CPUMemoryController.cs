using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPUMemoryController : MemoryLayer {

    public int programCounter = 0;

    private IEnumerator ClockTick()
    {
        while (true)
        {
            processInstruction();
            yield return new WaitForSeconds(1);
        }
    }

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(ClockTick());
    }
	
	// Update is called once per frame
	void Update () {
		// Check for new requests
	}

    public override void FulfillRequest()
    {

    }

    /*
     * Request an instruction from the layer above and execute it
     */
    public void processInstruction()
    {
        // request the next instruction from memory
        MakeRequest();
        programCounter++;
    }
}
