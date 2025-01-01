using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Escape : MonoBehaviour {
    [Tooltip("A game object whose children have a Target component. Each child represents a target.")]
    [SerializeField] private Transform targetFolder = null;

    [Tooltip("The player object.")]
    [SerializeField] private Transform player = null;

    private Target[] allTargets = null;
    private Target currentTarget = null;

    private NavMeshAgent navMeshAgent;
    private float rotationSpeed = 5f;
    private float recalculateTargetInterval = 0.5f; // Time interval to recalculate the target
    private float timeSinceLastRecalculation = 0f;
    private float playerProximityTrigger = 2f; // Distance to trigger recalculation when the player is close

    private bool isTargetChanged = false; // Track if the target has changed to limit logging

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Get all active targets in the folder
        allTargets = targetFolder.GetComponentsInChildren<Target>(false);
        Debug.Log("Escape state: Found " + allTargets.Length + " active targets.");
        SelectNewTarget(); // Select an initial target
    }

    private void Update() {
        timeSinceLastRecalculation += Time.deltaTime;

        // Recalculate the target periodically or if the player is close
        if (timeSinceLastRecalculation >= recalculateTargetInterval) {
            SelectNewTarget(); // Recalculate the furthest target
            timeSinceLastRecalculation = 0f;
        }

        // If the player is too close, force the enemy to recalculate
        if (DistanceToPlayer() < playerProximityTrigger) {
            SelectNewTarget();
        }

        // Update the movement
        if (navMeshAgent.hasPath) {
            FaceDestination();
        }
    }

    private void SelectNewTarget() {
        // Check for the furthest target
        float maxDistance = float.MinValue;
        Target furthestTarget = null;

        foreach (Target target in allTargets) {
            float distanceToPlayer = Vector3.Distance(target.transform.position, player.position);

            // Only log once when recalculating target or if a new target is selected
            // if (!isTargetChanged) {
            //     Debug.Log("Distance to target: " + target.name + " = " + distanceToPlayer);
            // }

            if (distanceToPlayer > maxDistance) {
                maxDistance = distanceToPlayer;
                furthestTarget = target;
            }
        }

        // Check if a new target has been selected
        if (furthestTarget != null && furthestTarget != currentTarget) {
            currentTarget = furthestTarget;
            navMeshAgent.destination = currentTarget.transform.position; // Update destination
            if (!isTargetChanged) {
                Debug.Log("Escape state: New furthest target selected: " + currentTarget.name);
                isTargetChanged = true; // Mark that the target has changed
            }
        } else {
            if (isTargetChanged) {
                Debug.Log("Escape state: No new target found, keeping current one.");
                isTargetChanged = false; // Reset target change flag
            }
        }
    }

    private void FaceDestination() {
        Vector3 directionToDestination = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToDestination.x, 0, directionToDestination.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private float DistanceToPlayer() {
        return Vector3.Distance(transform.position, player.position);
    }
}