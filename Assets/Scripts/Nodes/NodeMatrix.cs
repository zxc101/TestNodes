using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class NodeMatrix : MonoBehaviour
    {
        public Transform nodeBox;
        public Transform boxesBase;
        public Transform pet;
        public Color rawNodeColor;
        public Color nodeColor;
        public float wolkDistance;
        public float jumpDistance;
        public int count;
        public LayerMask layerMask;

        public Transform NodeBox => nodeBox;
        public Transform BoxesBase => boxesBase;
        public Transform Pet => pet;
        public Color RawNodeColor => rawNodeColor;
        public Color NodeColor => nodeColor;
        public float WolkDistance => wolkDistance;
        public float JumpDistance => jumpDistance;
        public LayerMask LayerMask => layerMask;

        public int Count { get => count; set => count = value; }

        public Transform Transform { get; private set; }

        public MoveManager   MoveManager   { get; private set; }
        public CreateManager CreateManager { get; private set; }

        private void InitManagers()
        {
            MoveManager   = new MoveManager  (this);
            CreateManager = new CreateManager(this);
        }

        private void InitComponents()
        {
            Transform = transform;
        }

        private void Start()
        {
            InitComponents();
            InitManagers();
        }
    }
}
