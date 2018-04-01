using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryElementController : PacketController, IComparable<MemoryElementController>{
    public int address = 0;
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

    public int CompareTo(MemoryElementController x)
    {
        return address.CompareTo(x.address);
    }

    protected override void FulfillRequest()
    {
        // the distination can now reply to its request
        destinationLayer.FulfillRequest(Address);
    }

    public class IComparer : IComparer<MemoryElementController>
    {
        public int Compare(MemoryElementController x, MemoryElementController y) {
            return x.address.CompareTo(y.address);
        }
    }
}