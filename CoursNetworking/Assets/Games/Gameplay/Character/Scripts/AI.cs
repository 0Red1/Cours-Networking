using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AI : NetworkBehaviour
{
    #region Variables
    public enum State
    {
        Wait,
        Walk,
        Fight
    }

    [Header("Search Target Logic")]
    [SerializeField] private Transform target;
    [SerializeField] private State state;
    [SerializeField] private float overlapRadius = 1f;
    [SerializeField] private float yOffset = 1f;

    [Header("Stagger Logic")]
    [SerializeField] private float staggerDuration = 0.5f;
    private float timeSinceHit = 999f;

    public HealthSystem healthSystem;
    public AttackDamageLogic attackDamageLogic;

    private NavMeshAgent _agent;
    private CharacterAnimationsController _characterAnimationsController;
    private CharacterSkillsPlayer _characterSkills;

    private float searchCooldown = 0.5f;
    private float timeSinceLastSearch = 0f;
    #endregion

    private GameObject ownerObject;


    #region Built-in Methods
    private void Awake()
    {
        if (attackDamageLogic != null)
        {
            attackDamageLogic.SetOwner(healthSystem);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _characterAnimationsController = GetComponent<CharacterAnimationsController>();
        _characterSkills = GetComponent<CharacterSkillsPlayer>();

        if (healthSystem != null) 
        {
            healthSystem.OnHit += StartStagger;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        timeSinceLastSearch += Time.deltaTime;
        timeSinceHit += Time.deltaTime;

        bool isStaggered = timeSinceHit < staggerDuration;

        if (isStaggered)
        {
            Debug.Log(gameObject.name + "se fait taper");
        }

        if (state == State.Wait)
        {
            _agent.SetDestination(transform.position);

            if (timeSinceLastSearch >= searchCooldown)
            {
                SearchTarget();
                timeSinceLastSearch = 0f;
            }
        }

        if (state == State.Walk)
        {
            _agent.SetDestination(target.position);
            _characterAnimationsController.SetSpeed(_agent.speed);

            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            if (distanceToPlayer <= 1)
            {
                if (!isStaggered)
                state = State.Fight;
                _characterAnimationsController.SetSpeed(0f);
            }
        }

        if (state == State.Fight)
        {
            _agent.SetDestination(transform.position);

            if (isStaggered)
            {
                return;
            }

            _characterSkills.BaseAttack();
        }
    }
    #endregion

    void SearchTarget()
    {
        Collider[] targets = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), overlapRadius);

        foreach (Collider other in targets)
        {
            if (other.gameObject == ownerObject)
            {
                continue;
            }

            if (other.gameObject.CompareTag("Player"))
            {
                target = other.gameObject.transform;
                state = State.Walk;
            }
        }
    }

    void StartStagger()
    {
        timeSinceHit = 0f;
        state = State.Wait;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), overlapRadius);
    }
}
