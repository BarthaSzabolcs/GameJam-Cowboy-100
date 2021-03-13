using UnityEngine;

using BarthaSzabolcs.CommonUtility;
using UnityEngine.AI;
using BarthaSzabolcs.RichTextHelper;

namespace GameJam.AI
{
    public class ChargingEnemy : MonoBehaviour
    {
        #region Enums

        private enum Behaviour { None, Wander, Charge };

        #endregion
        #region Datamembers

        #region Editor Settings

        [Header("Charge")]
        [SerializeField] private float chargeDuration;
        [SerializeField] private float chargeFrequency;
        [SerializeField] private float chargeRange;
        [SerializeField] private float chargeSpeed;

        [Header("Rest")]
        [SerializeField] private float wanderTime;
        [SerializeField] private float wamderDistance;
        [SerializeField] private float wanderSpeed;
        #endregion
        #region Public Properties

        [field: SerializeField]
        public AIBlackBoard BlackBoard { get; private set; }

        #endregion
        #region Private Properties

        private Behaviour State
        {
            get
            {
                return _state;
            }
            set
            {
                Debug.Log($"{name} changed state from {$"{_state}".Color(Color.magenta)} to {$"{value}".Color(Color.cyan)}");
                
                _state = value;

                switch (value)
                {
                    case Behaviour.None:
                        agent.destination = transform.position;
                        break;

                    case Behaviour.Wander:
                        agent.destination = RandomDestination();
                        agent.speed = wanderSpeed;
                        
                        wanderTimer.Reset();
                        break;

                    case Behaviour.Charge:
                        agent.destination = BlackBoard.Player.transform.position;
                        agent.speed = chargeSpeed;

                        chargeDurationTimer.Reset();
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion
        #region Private Fields

        private Behaviour _state;
        
        // Timers
        private Timer restTimer = new Timer();
        private Timer chargeDurationTimer = new Timer();
        private Timer wanderTimer = new Timer();

        private NavMeshAgent agent;

        #endregion

        #endregion

        void Start()
        {
            restTimer.Interval = chargeFrequency;
            restTimer.Init();

            chargeDurationTimer.Interval = chargeDuration;
            chargeDurationTimer.Init();

            wanderTimer.Interval = wanderTime;
            wanderTimer.Init();

            agent = GetComponent<NavMeshAgent>();
            State = Behaviour.Wander;
        }

        void Update()
        {
            restTimer.Tick(Time.deltaTime);

            StateLogic();
        }

        private void StateLogic()
        {
            switch (State)
            {
                case Behaviour.None:
                    break;

                case Behaviour.Wander:
                    Wander();
                    return;

                case Behaviour.Charge:
                    Charge();
                    break;
            }
        }

        private void Charge()
        {
            chargeDurationTimer.Tick(Time.deltaTime);

            if (chargeDurationTimer.Elapsed)
            {
                State = Behaviour.Wander;
            }
        }

        private void Wander()
        {
            wanderTimer.Tick(Time.deltaTime);

            if (wanderTimer.Elapsed)
            {
                agent.destination = RandomDestination();

                wanderTimer.Reset();
            }

            if (restTimer.Elapsed && PlayerInRange())
            {
                State = Behaviour.Charge;
            }
        }

        private Vector3 RandomDestination()
        {
            var randomVector = new Vector3(Random.Range(-1, 1), y: 0, z: Random.Range(-1, 1)).normalized;
            randomVector *= wamderDistance;

            return transform.position + randomVector;
        }

        private bool PlayerInRange()
        {
            var playerPosition = BlackBoard.Player.transform.position;
            var position = transform.position;

            var sqrDistance = Vector3.SqrMagnitude(playerPosition - position);

            return sqrDistance < chargeRange * chargeRange;
        }
    }
}