using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class MemoryLayerIntegration {
	private GameObject prefabLayerBase;

	/* Load the prefab resources from the assets folder
	 */
	[OneTimeSetUp]
	protected void LoadPrefabs ()
	{
		prefabLayerBase = Resources.Load("Prefabs/LayerBase", typeof(GameObject)) as GameObject;
		if(prefabLayerBase == null){
			Debug.Log("Couldn't load Prefab");
		}
	}

	/* After each test all game objects in the scene should be destroyed.
	 */
	[TearDown]
	protected void DeleteGameObjects ()
	{
		// get a list of all gameobjects
		var objects = Object.FindObjectsOfType<GameObject>();
		// destroy the objects
		foreach(GameObject obj in objects)
		{
			Object.Destroy(obj);
		}
	}

	/* Utility method to instantiate a mock memory layer
	 */
	private MemoryLayer MockLayer (Vector3 pos)
	{
		// Create the layer and its parent object
		MemoryLayer layer = new GameObject().AddComponent<MemoryLayer>();

		// set the position of the layers
		layer.gameObject.transform.position = pos;

		return layer;
	}

	/* When a memory layer component is created it should:
	 * Set its base size
	 * Create an empty list of memory locations
	 */
	[UnityTest]
	public IEnumerator CreateLayer ()
	{
		// get a mock layer
		MemoryLayer layer =  MockLayer(new Vector3(0, 0, 50));

		// release for a frame so that start can be called on the layer
		yield return null;

		// check that the memory locations list exists and is empty
		Assert.AreEqual(0, layer.memoryLocations.Count);
	}

	/* Create two memory layers, associate them with eachother and send a memory request
	 * between them. Checks that the request was made and that the requesting layer added
	 * the address to its list once it was fulfilled.
	 */
	[UnityTest]
	public IEnumerator SingleMemoryRequest ()
	{
		int testAddress = 0;

		// create two mock layers
		MemoryLayer upperLayer = MockLayer(new Vector3(50, 0, 50));
		MemoryLayer lowerLayer = MockLayer(new Vector3(0, 0, 50));

		// release for a frame so that start can be called on the layers
		yield return null;

		// associate the layers with eachother
		upperLayer.layerBelow = lowerLayer;
		lowerLayer.layerAbove = upperLayer;

		// set the layer latencies to 1 which will make the packets arrive in a single update
		upperLayer.layerLatency = 1;
		lowerLayer.layerLatency = 1;

		// add some memory addresses to the upper layer
		upperLayer.AddMemoryLocation(testAddress);

		// make some requests on the lower layer
		lowerLayer.MakeRequest(testAddress);

		// wait until the request has been fulfilled
		yield return new WaitForSeconds(1);

		// Check that the lower layer now has the address in its list
		Assert.IsTrue(lowerLayer.CheckForAddress(testAddress) >= 0);
	}

	/* Creates two layers, associates them with eachother and fills the upper with addresses.
	 * Make requests from the lower layer until it is full then tests that new requests follow the
	 * correct replacement policy.
	 */
	[UnityTest]
	public IEnumerator ElementReplacement ()
	{
		// setup the layers
		MemoryLayer upperLayer = MockLayer(new Vector3(50, 0, 50));
		MemoryLayer lowerLayer = MockLayer(new Vector3(0, 0, 50));

		// release for a frame so that start can be called on the layers
		yield return null;

		// associate the layers with eachother
		upperLayer.layerBelow = lowerLayer;
		lowerLayer.layerAbove = upperLayer;

		// set the layer latencies to 1 which will make the packets arrive in a single update
		upperLayer.layerLatency = 1;
		lowerLayer.layerLatency = 1;

		// add addresses to the upper layer
		upperLayer.AddMemoryLocation(0);
		upperLayer.AddMemoryLocation(1);
		upperLayer.AddMemoryLocation(2);
		upperLayer.AddMemoryLocation(3);
		upperLayer.AddMemoryLocation(4);

		// set the size of the lower layer to 3
		lowerLayer.SetSize(3);

		// make three calls to fill the lower layer
		lowerLayer.MakeRequest(0);
		lowerLayer.MakeRequest(1);
		lowerLayer.MakeRequest(2);

		// wait for the requests to be handled
		yield return new WaitForSeconds(1);

		// make a new request which should cause a replacement
		lowerLayer.MakeRequest(3);

		// wait for the request to be handled
		yield return new WaitForSeconds(1);

		// the lower layer should now contain the most recently requseted address
		Assert.IsTrue(lowerLayer.CheckForAddress(3) >= 0);
		// it should also only contain three elements
		Assert.IsTrue(3 == lowerLayer.memoryLocations.Count);
	}

	/* Creates a memory layer, adds some memory elements to it then clears it
	 */
	[UnityTest]
	public IEnumerator ClearingLayer ()
	{
		// setup the layer
		MemoryLayer layer = MockLayer(new Vector3(0, 0, 50));

		// release for a frame so that start can be called on the layer
		yield return null;

		// add addresses to the layer
		layer.AddMemoryLocation(0);
		layer.AddMemoryLocation(1);
		layer.AddMemoryLocation(2);
		layer.AddMemoryLocation(3);
		layer.AddMemoryLocation(4);

		// make sure they were added correctly
		Assert.IsTrue(layer.memoryLocations.Count == 5);

		// clear the layer
		layer.ClearLayer();

		// make sure the layer is now empty
		Assert.IsTrue(layer.memoryLocations.Count == 0);
	}
}