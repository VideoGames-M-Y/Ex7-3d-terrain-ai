using UnityEngine;

/**
 * This component manages the enemy's state machine, switching between guarding, escaping, and moving to the engine.
 */
[RequireComponent(typeof(Guard))]
[RequireComponent(typeof(Escape))]
[RequireComponent(typeof(GetEngine))]
public class EnemyControllerStateMachine : StateMachine
{
    [SerializeField] private float radiusToWatch = 5f; // Radius within which the enemy reacts to the player
    [SerializeField] private float probabilityToEscape = 0.2f; // Probability of switching to escape mode
    [SerializeField] private float probabilityToGuard = 0.2f; // Probability of switching back to guard mode

    private Guard guard;
    private Escape escape;
    private GetEngine getEngine;

    private float DistanceToPlayer() {
        // Access the player's position from the Guard script
        return Vector3.Distance(transform.position, guard.player.position);
    }

    private void Awake() {
        guard = GetComponent<Guard>();
        escape = GetComponent<Escape>();
        getEngine = GetComponent<GetEngine>();

        base
            .AddState(guard)         // Ensure Guard is the first state added
            .AddState(escape)
            .AddState(getEngine)
            .AddTransition(guard, () => DistanceToPlayer() <= radiusToWatch, escape)
            .AddTransition(escape, () => Random.Range(0f, 1f) < probabilityToGuard * Time.deltaTime, guard)
            .AddTransition(escape, () => DistanceToPlayer() > radiusToWatch, guard)
            .AddTransition(guard, () => Random.Range(0f, 1f) < probabilityToEscape * Time.deltaTime, escape)
            .AddTransition(guard, () => DistanceToPlayer() > radiusToWatch, getEngine)
            .AddTransition(getEngine, () => DistanceToPlayer() <= radiusToWatch, escape);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusToWatch);
    }
}