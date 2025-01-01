using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Guard : MonoBehaviour
{
    [Tooltip("A game object whose children have a Target component. Each child represents a target.")]
    [SerializeField] private Transform targetFolder = null;

    [Tooltip("The player transform for calculating proximity.")]
    [SerializeField] public Transform player = null;

    private Target[] allTargets = null;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Target currentTarget = null;
    private float rotationSpeed = 5f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Get all active targets
        allTargets = targetFolder.GetComponentsInChildren<Target>(false);
        Debug.Log($"Found {allTargets.Length} active targets.");
    }

    private void Update()
    {
        // Continuously find the closest target to the player's current position
        Target closestTarget = GetClosestTargetToPlayer();

        // If the closest target is different from the current target, update the destination
        if (closestTarget != null && closestTarget != currentTarget)
        {
            currentTarget = closestTarget;
            navMeshAgent.destination = currentTarget.transform.position;
            Debug.Log($"Guard state: Moving to closest target: {currentTarget.name}");
        }

        // If the player is moving, ensure the guard keeps recalculating its destination
        if (currentTarget != null)
        {
            navMeshAgent.destination = currentTarget.transform.position;
        }

        // Rotate toward the target destination if the guard has a path
        if (navMeshAgent.hasPath)
        {
            FaceDestination();
        }
    }

    private Target GetClosestTargetToPlayer()
    {
        if (allTargets == null || allTargets.Length == 0) return null;

        Target closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (Target target in allTargets)
        {
            // Calculate distance between player and each target
            float distanceToPlayer = Vector3.Distance(player.position, target.transform.position);

            // Debug log: Show distance to each target
            Debug.Log($"Target: {target.name}, Distance to Player: {distanceToPlayer}");

            // Find the closest target by distance
            if (distanceToPlayer < minDistance)
            {
                minDistance = distanceToPlayer;
                closestTarget = target;
            }
        }

        // Debug log: Show which target is currently the closest
        if (closestTarget != null)
        {
            Debug.Log($"Guard state: Closest Target: {closestTarget.name}, Distance: {minDistance}");
        }

        return closestTarget;
    }

    private void FaceDestination()
    {
        // Look at the destination with only the X and Z axes
        Vector3 directionToDestination = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToDestination.x, 0, directionToDestination.z));

        // Rotate gradually toward the destination
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        Debug.Log("Guard state: Rotating to face destination.");
    }
}