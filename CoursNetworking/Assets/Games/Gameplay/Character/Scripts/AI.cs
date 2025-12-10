using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    #region Variables
    public enum State 
    {
        Wait,
        Walk,
        Fight
    }

    [SerializeField] private Transform testTarget;
    [SerializeField] private State state;

    private NavMeshAgent _agent;
    #endregion

    #region Built-in Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Wait) 
        { 
            _agent.SetDestination(transform.position);
        }

        if (state == State.Walk)
        {
            _agent.SetDestination(testTarget.position);
        }

        if (state == State.Fight) 
        { 
            _agent.SetDestination(transform.position);
            // taper
        }
    }
    #endregion
}
