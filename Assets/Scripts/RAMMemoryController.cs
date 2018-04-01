using System.Collections; using System.Collections.Generic; using UnityEngine;  public class RAMMemoryController : MemoryLayer {      public override void Awake()
    {
        base.Awake();
        LoadProgramFromDisk();
    }     // Causes the cpu to ask the next memory layer for a chunk of memory
    //public override void MakeRequest(int address)
    //{
        // Set the address of the memory being asked for

        // animate the packet after this layers latency
        //FulfillRequest(address);
    //}      public void LoadProgramFromDisk ()     {         AddMemoryLocation(0);         AddMemoryLocation(1);         AddMemoryLocation(2);         AddMemoryLocation(3);         AddMemoryLocation(4);         AddMemoryLocation(5);         AddMemoryLocation(6);         AddMemoryLocation(7);         AddMemoryLocation(8);         AddMemoryLocation(9);     }      public override void ToggleLayer()
    {
        return;
    } } 