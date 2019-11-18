using System.Collections.Generic;
using UnityEngine;

using Helpers;
using Collections;

namespace Pets
{
    public class Pet : MonoBehaviour
    {
        public float TIME_ROTATE => 4;
        public float TIME_MOVE => 4;

        [Header("Цели")]
        [SerializeField] private Transform feeder;
        [SerializeField] private Transform bed;
        [Header("Потребности")]
        [SerializeField] [Range(0, 100)] private float needEat;
        [SerializeField] [Range(0, 100)] private float needSleep;
        [Tooltip("Префаб для вспомогательных целей")]
        [SerializeField] private Transform point;

        [Header("Other")]
        [SerializeField] private Transform eye;
        
        private float speedMove;
        private float speedRotate;
        private bool isRotate;

        public Stack<Vector3> Path { get; set; }

        public Transform Feeder => feeder;
        public Transform Bed => bed;
        public Transform Point => point;
        public Transform Eye => eye;
        public float NeedEat => needEat;
        public float NeedSleep => needSleep;
        public float AngleToGoal => Goals.IsEmpty ? 0 : MathHelper.Angle(transform, Goals.First.position);

        public Deque<Transform> Goals { get; set; }
        public float SpeedMove { get => speedMove; set { speedMove = value; Animator.SetFloat("Speed", value); } }
        public float SpeedRotate { get => speedRotate; set { speedRotate = value; Animator.SetFloat("Rotate", value); } }

        public Transform Transform { get; private set; }
        public Animator Animator { get; private set; }
        public CapsuleCollider CapsuleCollider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        public AnimManager AnimManager { get; private set; }
        public GoalManager GoalManager { get; private set; }
        public JumpManager JumpManager { get; private set; }
        public MoveManager MoveManager { get; private set; }
        public SitManager SitManager { get; private set; }

        private void InitVal()
        {
            Goals = new Deque<Transform>();
        }

        private void InitComponents()
        {
            Transform = transform;
            Animator = GetComponent<Animator>();
            CapsuleCollider = GetComponent<CapsuleCollider>();
            Rigidbody = GetComponent<Rigidbody>();
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
            InitVal();
            InitComponents();
            InitManagers();
        }

        public Vector3 NextPosition
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
                float MAX_Speed = Goals.IsEmpty ? 0 : Vector3.Distance(transform.position, Goals.First.position);
                MAX_Speed = Mathf.Clamp(MAX_Speed, 0, 2);
                return MAX_Speed;
            }
        }

        public void ClearAllGoals()
        {
            if (Goals != null)
            {
                RemoveGoals(Goals.Count);
            }
        }

        public void RemoveAllHalperGoals()
        {
            RemoveGoals(Goals.Count - 1);
        }

        public void ChangeMainGoal(Transform newPoint)
        {
            if (Goals != null && !Goals.IsEmpty)
                Destroy(Goals.RemoveLast().gameObject);
            Goals.AddLast(Instantiate(Point, newPoint.position, Quaternion.identity));
        }
        
        public void RemoveMainGoal()
        {
            if (Goals != null && !Goals.IsEmpty)
            {
                if (Goals.Last.position == bed.position ||
                Goals.Last.position == feeder.position)
                {
                    Destroy(Goals.RemoveLast().gameObject);
                }
            }
        }

        private void RemoveGoals(int count)
        {
            for (int i = 0; i < count; i++)
            {
                RemoveGoal();
            }
        }

        private void RemoveGoal()
        {
            Destroy(Goals.RemoveFirst().gameObject);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.tag == "Goal")
            {
                if (Goals != null && !Goals.IsEmpty) RemoveGoal();
            }
        }
    }
}
