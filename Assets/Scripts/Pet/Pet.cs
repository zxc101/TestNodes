using System.Collections.Generic;
using UnityEngine;

using Helpers;
using Collections;
using System.Collections;

namespace Pets
{
    public class Pet : MonoBehaviour
    {
        public float TIME_ROTATE => 4;
        public float TIME_MOVE => 4;

        [SerializeField] private float timeConsumptionNeeds;

        [Header("Потребности")]
        public Need[] Needs;

        [Header("Префаб для вспомогательных целей")]
        [SerializeField] private Transform point;

        [Header("Other")]
        [SerializeField] private Transform eye;
        
        private float speedMove;
        private float speedRotate;

        public Stack<Vector3> Path { get; set; }

        [HideInInspector] public Need Need;
        [HideInInspector] public Transform Transform;

        public float AngleToGoal => Goals.IsEmpty ? 0 : MathHelper.Angle(transform, Goals.First.position);
        
        public float TimeConsumptionNeeds => timeConsumptionNeeds;
        public Transform Point => point;
        public Transform Eye => eye;

        public Deque<Transform> Goals { get; set; }
        public float SpeedMove { get => speedMove; set { speedMove = value; Animator.SetFloat("Speed", value); } }
        public float SpeedRotate { get => speedRotate; set { speedRotate = value; Animator.SetFloat("Rotate", value); } }

        public Animator Animator { get; private set; }
        public CapsuleCollider CapsuleCollider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        public AnimManager AnimManager { get; private set; }
        public GoalManager GoalManager { get; private set; }
        public JumpManager JumpManager { get; private set; }
        public MoveManager MoveManager { get; private set; }
        public NeedsManager NeedsManager { get; private set; }
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
            AnimManager  = new AnimManager(this);
            GoalManager  = new GoalManager(this);
            JumpManager  = new JumpManager(this);
            MoveManager  = new MoveManager(this);
            NeedsManager = new NeedsManager(this);
            SitManager   = new SitManager(this);
        }

        private void Start()
        {
            InitVal();
            InitComponents();
            InitManagers();
            StartCoroutine(NeedsManager.Consumptions());
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

        public void RemoveFirstGoal()
        {
            Destroy(Goals.RemoveFirst().gameObject);
        }

        public void RemoveLastGoal()
        {
            Destroy(Goals.RemoveLast().gameObject);
        }

        public void AddFirstGoal(Transform newPoint)
        {
            Goals.AddFirst(Instantiate(Point, newPoint.position, Quaternion.identity));
        }

        public void AddLastGoal(Transform newPoint)
        {
            Goals.AddLast(Instantiate(Point, newPoint.position, Quaternion.identity));
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.tag == "Goal")
            {
                if (collider.transform.position == Need.prefab.position &&
                    Goals.Last.position == Need.prefab.position &&
                    Need.value < Need.criticalValue)
                {
                    AnimManager.Need(Need.name, true);
                    StartCoroutine(NeedsManager.Processing());
                }
            }

            if (Goals != null && !Goals.IsEmpty && collider.transform.position == Goals.Last.position)
            {
                GoalManager.ClearAllGoals();
            }
        }
    }
}
