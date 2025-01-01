using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GetEngine : MonoBehaviour {
    [Tooltip("The engine GameObject the enemy is trying to reach.")]
    [SerializeField] private Transform engine;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    [SerializeField] private float rotationSpeed = 5f;

    // Minimum distance to consider the agent has reached the engine
    [SerializeField] private float arrivalThreshold = 0.5f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (engine == null)
        {
            Debug.LogError("GetEngine state: Engine target is not assigned!");
            return;
        }

        // Set stopping distance to a small value
        navMeshAgent.stoppingDistance = 0.1f;

        // Set the engine as the destination
        navMeshAgent.destination = engine.position;
        Debug.Log($"GetEngine state: Moving to the engine: {engine.name}");
    }

    private void Update()
    {
        if (navMeshAgent.hasPath && !HasReachedEngine())
        {
            FaceDestination();
        }
        else if (HasReachedEngine())
        {
            Debug.Log("GetEngine state: Reached the engine!");
            // Add logic for what happens when the enemy reaches the engine
        }
    }

    private bool HasReachedEngine()
    {
        // Check if the distance to the engine is less than the arrival threshold
        float distanceToEngine = Vector3.Distance(transform.position, engine.position);
        return distanceToEngine <= arrivalThreshold;
    }

    private void FaceDestination()
    {
        Vector3 directionToDestination = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToDestination.x, 0, directionToDestination.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // Gradual rotation
        Debug.Log("GetEngine state: Rotating to face destination.");
    }
}