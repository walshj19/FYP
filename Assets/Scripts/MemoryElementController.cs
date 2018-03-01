using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryElementController : PacketController {
    private int address = 0;
    public int Address
    {
        get
        {
            return address;
        }

        set
        {
            address = value;
            transform.Find("Address Label").gameObject.GetComponent<TextMesh>().text = address.ToString();
        }
    }

    protected override void FulfillRequest()
    {
        // the distination can now reply to its request
        destinationLayer.FulfillRequest(Address);
    }
}