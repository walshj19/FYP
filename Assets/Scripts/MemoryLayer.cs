using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Base class for all memory layers. Holds general state such as size and methods for memory access.
 */
public class MemoryLayer : MonoBehaviour
{
	public int absoluteLayerNumber;                         // The absolute position of this layer in the memory heirarchy
	public GameObject packetPrefab;                         // The prefab that will be used when generating packets
	public int layerLatency = 10;                           // The time it will take for packets from this layer to reach the next layer
	public int maxWidth = 10;                               // maximum number of memory elements in a row
	public int size = 10;                                   // current max number of elements in this layer
	public int maxSize = 300;                               // absolute max number of elements this layer can have
	public GameObject layerBase;                            // The prefab used for the layer base shape
	public MemoryLayer layerAbove;                          // layer requests will be passed to
	public MemoryLayer layerBelow;                          // layer requests will be received from
	public List<MemoryElementController> memoryLocations;   // Contains the addresses stored in this layer
	public bool layerDisabled = false;                      // if disabled this layer won't send or receive requests
	private GameObject baseShape;                           // reference to the base shape object for this layer

	public virtual void Awake()
	{
		// Declare the list of memory locations
		memoryLocations = new List<MemoryElementController>();

		// Load the prefabs
		layerBase = Resources.Load("Prefabs/LayerBase", typeof(GameObject)) as GameObject;
		packetPrefab = Resources.Load("Prefabs/Ram Memory Element", typeof(GameObject)) as GameObject;

		if (layerBase == null || packetPrefab == null)
		{
			Debug.Log("Couldn't load MemoryLayer prefabs");
		}
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

	/* Creates the base shape for the layer based on its size
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

	/* Creates all of the memory elements for this layer based on its size
	 */
	private void CreateMemoryElements()
	{
		// add memory blocks based on size
		// so if this is ram and the size is 1024 it should add 1024 memory blocks and arrange them appropriately
		// memory blocks should be layed out in a fixed width grid 
		//foreach (int address in memoryLocations)
		//{
		//    int listPosition = memoryLocations.BinarySearch(address);
			// calculate where the element should be placed

		//    Vector3 pos = transform.position;
		//    pos.z -= (float)System.Math.Floor((double)listPosition / maxWidth);
		//    pos.x += (listPosition%maxWidth) * 3;
		//    pos.y = 1;

			//place one element
		//    GameObject elem = Instantiate<GameObject>(memoryElement, pos, transform.rotation, transform);
		//    // calculate current element number and set address field
		//    elem.transform.Find("Address Label").gameObject.GetComponent<TextMesh>().text = address.ToString();
		//}
	}

	/* Causes the cpu to ask the next memory layer for a chunk of memory
	 */
	public virtual void MakeRequest(int address)
	{
		Debug.Log("Layer "+absoluteLayerNumber+": Request received for address="+address);
		if (CheckForAddress(address) >= 0)
		{
			Debug.Log("Layer " + absoluteLayerNumber + ": Have cached address=" + address);
			FulfillRequest(address);
		}
		else
		{
			Debug.Log("Layer " + absoluteLayerNumber + ": Address not in cache, passing on request for address=" + address);
			// animate the packet after this layers latency
			layerAbove.MakeRequest(address);
		}
	}

	public virtual void FulfillRequest(int address)
	{
		if (layerBelow != null)
		{
			AnimateRequest(address);
		}

		Debug.Log("Layer " + absoluteLayerNumber + ": Fulfilling request for address=" + address);
		// if the address isn't in this layers cache yet add it
		if (CheckForAddress(address) < 0)
		{
			Debug.Log("Layer " + absoluteLayerNumber + ": Address not in cache, adding to cache address=" + address);
			AddMemoryLocation(address);
		}
	}

	public void AddMemoryLocation(int address)
	{
		// check if the layer is full
		if (memoryLocations.Count >= size)
		{
			Debug.Log("Layer " + absoluteLayerNumber + ": Cache full removing element");
			//remove an element
			RemoveMemoryLocation();
		}
		//create the address element
		MemoryElementController packet = Instantiate<GameObject>(packetPrefab).GetComponent<MemoryElementController>();
		packet.Address = address;
		//add the new element
		memoryLocations.Add(packet);
		//set the position of the new element
		int pos = CheckForAddress(address);
		Debug.Log("Layer " + absoluteLayerNumber + " Creating element object at index:" + pos+" for address="+address);
		packet.transform.SetPositionAndRotation(GetPositionOfElement(address), transform.rotation);
	}

	private void RemoveMemoryLocation()
	{
		int rand = Random.Range(0, memoryLocations.Count);
		RemoveMemoryLocation(rand);
	}

	private void RemoveMemoryLocation(int index)
	{
		Debug.Log("Layer " + absoluteLayerNumber + ": Removing element at index="+index);
		// get a reference to the location to be removed
		MemoryElementController elem = memoryLocations[index];
		// remove it from the list
		memoryLocations.RemoveAt(index);
		// delete it's game object
		Destroy(elem.gameObject);
		//resort the list
		memoryLocations.Sort();
		memoryLocations.TrimExcess();
	}

	/* Returns the address of the item if found or -1 if not
	 */
	public int CheckForAddress(int address)
	{
		int index = -1;
		for (int i = 0; i < memoryLocations.Count; i++)
		{
			if (memoryLocations[i].Address == address)
			{
				index = i;
			}
		}
		Debug.Log("Layer " + absoluteLayerNumber + ": Result of search for address="+address+" is="+index);
		return index;
	}

	/* Method to animate a packet from the above layer to this layer
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

	/* Disable the layer so that requests pass through it
	 */
	public virtual void ToggleLayer()
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

	/* Get the vector location of a specific memory address in this layer
	 */
	public virtual Vector3 GetPositionOfElement(int index)
	{
		Vector3 pos = transform.position;
		pos = transform.position;
		pos.y = 2;

		pos.z -= (float)(index/maxWidth);
		pos.x += (index%maxWidth)*3;

		Vector3 tmp = transform.position;
		Debug.Log("Layer "+absoluteLayerNumber+": new element created, index"+index+" base position x="+tmp.x+" z="+tmp.z+" new position x="+pos.x+" z="+pos.z);

		return pos;
	}

	/* Handles the UI size slider being moved and updates the size of the layer.
	 */
	public void UpdateSize(float sliderValue)
	{
		size = (int)(sliderValue * (float)maxSize);
		//Debug.Log(slider.value+" "+maxSize+" "+size);

		UpdateSizeLabel();
		UpdateBase();
		UpdateMemoryLocations();
	}

	/* Handles an internal update to the size, does not update the UI.
	 */
	public void SetSize (int newSize)
	{
		size = newSize;

		//SetSizeSliderPosition();
		//UpdateSizeLabel();
		UpdateBase();
		UpdateMemoryLocations();
	}

	/* Sets the UI size label to the value of size.
	 */
	public void UpdateSizeLabel()
	{
		GameObject.FindGameObjectWithTag("Layer Size Value").GetComponent<Text>().text = size.ToString();
	}

	/* Positions the UI size slider to represent the current size.
	 */
	public void SetSizeSliderPosition()
	{
		float position = (float)size / (float)maxSize;
		Slider slider = GameObject.FindGameObjectWithTag("SizeSlider").GetComponent<Slider>();
		slider.value = position;
	}

	/* Adjusts the size of the memory locations list to match size, removing
	 * elements if necessary.
	 */
	private void UpdateMemoryLocations()
	{
		//if the list is longer than the length shrink it
		if (memoryLocations.Count > size)
		{
			Debug.Log("size:" + size + ", Count:"+memoryLocations.Count);
			//discard extra elements
			for (int i = size; i < memoryLocations.Count; i++)
			{
				RemoveMemoryLocation(i);
			}
		}
	}
}