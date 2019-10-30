using System.Collections.Generic;
using UnityEngine;

using Helpers;
using Nodes;

namespace Pets
{
    public class Pet : MonoBehaviour
    {
        public float TIME_ROTATE => 4;
        public float TIME_MOVE => 4;

        [SerializeField] private Transform currentGoal;
        [SerializeField] private Transform eye;
        [SerializeField] private bool exploreTerritory;

        private float speedMove;
        private float speedRotate;

        public Transform Eye => eye;
        public Animator Animator => GetComponent<Animator>();
        public Stack<Vector3> Path => Pathfinding.FindPath(transform.position, CurrentGoal.position);
        public float AngleToGoal => CurrentGoal == null ? 0 : MathHelper.Angle(transform, CurrentGoal.position);
        public bool ExploreTerritory => exploreTerritory;

        public Transform CurrentGoal { get => currentGoal; set => currentGoal = value; }
        public float SpeedMove { get => speedMove; set { speedMove = value; Animator.SetFloat("Speed", value); } }
        public float SpeedRotate { get => speedRotate; set { speedRotate = value; Animator.SetFloat("Rotate", value); } }

        public Transform Transform { get; private set; }

        public AnimManager AnimManager { get; private set; }
        public GoalManager GoalManager { get; private set; }
        public JumpManager JumpManager { get; private set; }
        public MoveManager MoveManager { get; private set; }
        public SitManager SitManager { get; private set; }

        private void InitComponents()
        {
            Transform = transform;
        }

        private void InitManagers()
        {
            AnimManager = new AnimManager(this);
            GoalManager = new GoalManager(this);
            JumpManager = new JumpManager(this);
            MoveManager = new MoveManager(this);
            SitManager = new SitManager(this);
        }

        private void Start()
        {
            InitComponents();
            InitManagers();
        }

        public Vector3 Goal
        {
            get
            {
                if (Vector3.Distance(transform.position, Path.Peek()) < 0.2f)
                {
                    return Path.Pop();
                }
                else
                {
                    return Path.Peek();
                }
            }
        }

        public float MaxSpeed
        {
            get
            {
                float MAX_Speed = Vector3.Distance(transform.position, currentGoal.position);
                MAX_Speed = Mathf.Clamp(MAX_Speed, 1, 2);
                return MAX_Speed;
            }
        }

        private void OnDrawGizmos()
        {
            if (Path != null)
            {
                Gizmos.color = Color.red;
                Vector3[] gizmosPath = Path.ToArray();
                for (int i = 0; i < gizmosPath.Length; i++)
                {
                    Gizmos.DrawCube(gizmosPath[i], Vector3.one * 0.2f);
                }
            }
        }
    }
}
