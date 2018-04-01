using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class MemoryLayerIntegration {
    private GameObject prefabLayerBase;

    [OneTimeSetUp]
    protected void LoadPrefabs ()
    {
        prefabLayerBase = Resources.Load("Prefabs/LayerBase", typeof(GameObject)) as GameObject;
        if(prefabLayerBase == null){
            Debug.Log("Couldn't load Prefab");
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
        // set the layer base objects to a rect
        layer.layerBase = GameObject.Instantiate(prefabLayerBase);
        layer.layerBase.transform.parent = layer.transform;

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

    [UnityTest]
    public IEnumerator SingleMemoryRequest ()
    {
        // create two mock layers
        MemoryLayer upperLayer = MockLayer(new Vector3(0, 0, 50));
        MemoryLayer lowerLayer = MockLayer(new Vector3(50, 0, 50));

        // release for a frame so that start can be called on the layers
        yield return null;

        // associate the layers with eachother
        upperLayer.layerBelow = lowerLayer;
        lowerLayer.layerAbove = upperLayer;

        // 
    }
}
