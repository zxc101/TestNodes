using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Nodes
{
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}
