using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float actionTimeout = 10f;

    [Header("AI Properties")]
    public float movementSpeed = 500f;
    public float wanderRadius = 5f;
    public float fleeMultiplier = 3f;
    public float fleeDistance = 15f;
    public GameObject target = null;
    [HideInInspector] public Vector3 moveTo = Vector3.zero;

    [HideInInspector] public bool reached = true;
    [HideInInspector] public bool pathIsValid = false;
    [HideInInspector] public bool undergoingAction = false;

    public static bool debugMode = true;

    private void Start()
    {
        agent.speed = movementSpeed;
        PerformAction();
    }

    private void Update()
    {
        // Calculate the distance. If not assume they reached their destination
        reached = agent.hasPath ? agent.remainingDistance <= 1f : true;
        agent.isStopped = agent.velocity != Vector3.zero;

    }

    public abstract void PerformAction();

    public abstract void OnAttack();

    public virtual IEnumerator Wander()
    {
        undergoingAction = true;
        moveTo = RandomNavmeshLocation(wanderRadius);
        agent.SetDestination(moveTo);
        yield return new WaitForEndOfFrame();
        bool path = agent.CalculatePath(moveTo, agent.path);
        if (path && agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            pathIsValid = false;
            PerformAction();
            undergoingAction = false;
            yield break;
        }
        pathIsValid = true;
        float timeout = Time.time + actionTimeout;
        yield return new WaitUntil(() => reached || (Time.time > timeout && agent.velocity != Vector3.zero));
        PerformAction();
        undergoingAction = false;
    }

    public virtual IEnumerator Search()
    {
        yield return null;
    }

    public virtual IEnumerator Persue()
    {
        if (target == null)
        {
            PerformAction();
            yield break;
        }
        yield return null;
    }

    public virtual IEnumerator Attack()
    {
        if (target == null)
        {
            PerformAction();
            yield break;
        }
        yield return null;
    }

    public virtual IEnumerator Flee()
    {
        undergoingAction = true;
        moveTo = transform.position + -transform.forward * fleeMultiplier;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(moveTo, out hit, fleeDistance, 1))
        {
            moveTo = hit.position;
            agent.SetDestination(moveTo);
            float timeout = Time.time + actionTimeout;
            yield return new WaitForEndOfFrame();
            bool path = agent.CalculatePath(moveTo, agent.path);
            if (path && agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                pathIsValid = false;
                PerformAction();
                undergoingAction = false;
                yield break;
            }
            pathIsValid = true;
            yield return new WaitUntil(() => reached || Time.time > timeout);
            PerformAction();
            undergoingAction = false;
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void OnDrawGizmos()
    {
        if (pathIsValid && debugMode)
        {
            // Draw at destination
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(moveTo, 0.25f);

            //Draw path
            var line = GetComponent<LineRenderer>();
            if (line == null)
            {
                line = this.gameObject.AddComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
                line.startColor = Color.yellow;
                line.endColor = Color.yellow;
            }

            var path = agent.path;

            line.positionCount = path.corners.Length;

            for (int i = 0; i < path.corners.Length; i++)
            {
                line.SetPosition(i, path.corners[i]);
            }

        }
    }
}
