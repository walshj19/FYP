using System.Collections; using System.Collections.Generic; using UnityEngine;  public class RAMMemoryController : MemoryLayer {     // Causes the cpu to ask the next memory layer for a chunk of memory
    public override void MakeRequest(int address)
    {
        // Set the address of the memory being asked for

        // animate the packet after this layers latency
        FulfillRequest(address);
    }      void LoadProgramFromDisk ()     {      } } 