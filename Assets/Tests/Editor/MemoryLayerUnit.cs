using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MemoryLayerUnit {

	[Test]
	public void MemoryLayerTestSimplePasses()
    {
		// Use the Assert class to test conditions.
	}

    [Test]
    public void SetupTest()
    {
        // Create a new memory layer
        MemoryLayer layer = new MemoryLayer();

        // 
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator MemoryLayerTestWithEnumeratorPasses()
    {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
