using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CPUMemoryController : MemoryLayer {
	public int programCounter = 0;
	public float clockSpeed = 1;
	public bool running = true;
	public bool waitingForRequest = false;

	private IEnumerator ClockTick()
	{
		while (true)
		{
			processInstruction();
			yield return new WaitUntil(() => !waitingForRequest);
		}
	}

	// Use this for initialization
	public override void Start () {
		StartCoroutine(ClockTick());
	}
	
	// Update is called once per frame
	void Update () {
		// Check for new requests
	}

	public override void FulfillRequest(int address)
	{
		// set the waiting flag so that the next request can be made
		waitingForRequest = false;

		// update the program counter
		if (programCounter >= 9)
		{
			programCounter = 0;
		}
		else
		{
			programCounter++;
		}
		// update program counter ui
		//GameObject.FindGameObjectWithTag("ProgramCounter").GetComponent<TextMesh>().text = programCounter.ToString();
		GameObject.FindGameObjectWithTag("Program Counter").GetComponent<Text>().text = programCounter.ToString();
	}

	public override Vector3 GetPositionOfElement(int address)
	{
		return transform.position;
	}

	/*
	 * Request an instruction from the layer above and execute it
	 */
	public void processInstruction()
	{
		// Cycle the first ten addresses
		MakeRequest(programCounter);

		// set the wait flag for the coroutine
		waitingForRequest = true;
	}

	public void ToggleRunning ()
	{
		running = !running;
	}
}