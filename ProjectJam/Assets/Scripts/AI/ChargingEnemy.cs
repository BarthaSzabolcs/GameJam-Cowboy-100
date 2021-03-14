using UnityEngine;
using UnityEngine.AI;

using BarthaSzabolcs.CommonUtility;
using BarthaSzabolcs.RichTextHelper;

using GameJam.HealthSystem;

namespace GameJam.AI
{
    public class ChargingEnemy : MonoBehaviour
    {
        #region Enums

        private enum Behaviour { None, Wander, Charge };

        #endregion
        #region Datamembers

        #region Editor Settings

        [Header("Components")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Health health;
        [SerializeField] private AIAnimatorHelper animator;

        [Header("Charge")]
        [SerializeField] private float chargeDuration;
        [SerializeField] private float chargeFrequency;
        [SerializeField] private float chargeRange;
        [SerializeField] private float chargeSpeed;

        [Header("Rest")]
        [SerializeField] private float wanderTime;
        [SerializeField] private float wanderDistance;
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
                        
                        animator.Calm = true;
                        break;

                    case Behaviour.Wander:
                        agent.destination = RandomDestination();
                        agent.speed = wanderSpeed;
                        
                        wanderTimer.Reset();

                        animator.Calm = false;
                        break;

                    case Behaviour.Charge:
                        agent.destination = BlackBoard.Player.transform.position;
                        agent.speed = chargeSpeed;

                        chargeDurationTimer.Reset();

                        animator.Calm = false;
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

        #endregion

        #endregion

        private void OnDrawGizmos()
        {
            if (agent != null && Application.isPlaying)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(agent.destination, 0.125f);
            }
        }

        void Start()
        {
            restTimer.Interval = chargeFrequency;
            restTimer.Init();

            chargeDurationTimer.Interval = chargeDuration;
            chargeDurationTimer.Init();

            wanderTimer.Interval = wanderTime;
            wanderTimer.Init();

            State = Behaviour.Wander;

            health.OnCalm += () => State = Behaviour.None;
        }

        void Update()
        {
            restTimer.Tick(Time.deltaTime);

            StateLogic();
            transform.position = agent.transform.position;
            animator.RefreshDirection(agent.transform.forward);
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
            var randomVector = new Vector3(Random.Range(-1f, 1f), y: Random.Range(-1f, 1f), z: 0).normalized;
            randomVector *= wanderDistance;

            return transform.position + randomVector;
        }

        private bool PlayerInRange()
        {
            var playerPosition = BlackBoard.Player.transform.position;
            var position = transform.position;

            var sqrDistance = Vector2.SqrMagnitude(playerPosition - position);

            return sqrDistance < chargeRange * chargeRange;
        }
    }
}