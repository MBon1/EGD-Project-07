using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Flee(target.transform.position);
        Pursue();
        //Evade();
        //Wander();
        //PathFollowing();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float targetSpeed = target.GetComponent<Drive>().currentSpeed;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20) || targetSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + targetSpeed);

        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float targetSpeed = target.GetComponent<Drive>().currentSpeed;

        float lookAhead = targetDir.magnitude / (agent.speed + targetSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }


    float wanderRadius = 10;
    float wanderDistance = 10;
    float wanderJitter = 1;
    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    [SerializeField] Transform[] pointsOnPath = { };
    [SerializeField] PathFollowingTypes pathFollowingType = PathFollowingTypes.OneTime;
    int nextPointIndex = 0;
    void PathFollowing()
    {
        if (nextPointIndex >= pointsOnPath.Length)
        {
            if (pathFollowingType == PathFollowingTypes.OneTime)
            {
                return;
            }
            else
            {
                nextPointIndex %= pointsOnPath.Length;
            }
        }

        /*for(int i = 0; i < pointsOnPath.Length; i++)
        {

        }*/

        Vector3 nextPoint = pointsOnPath[nextPointIndex].position;
        nextPoint = new Vector3(nextPoint.x, this.transform.position.y, nextPoint.z);
        Seek(nextPoint);

        if (Vector3.Distance(this.transform.position, nextPoint) <= agent.stoppingDistance)
        {
            nextPointIndex++;
        }
    }

    private enum PathFollowingTypes
    { 
        OneTime,
        Cyclical
    }
}
