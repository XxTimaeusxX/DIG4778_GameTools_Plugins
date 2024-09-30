using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Avoider : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject avoidee; 
    public float avoidRange = 5f; 
    public float speed = 3.5f; 
    private float samplingRadius = 2f; 

    private List<Vector3> sampledPoints = new List<Vector3>(); 
     public bool showGizmos = true; 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogWarning("This GameObject does not have a NavMesh Agent. Please add one and bake a NavMesh.");
            return;
        }

        if (avoidee == null)
        {
            Debug.LogWarning("Avoidee object is not assigned. Please assign an object in the inspector.");
        }
    }

    private void Update()
    {
       
        if (avoidee != null)
        {
            MaintainEyeContact();
            // Check if the avoidee is within the avoidance range
            if (Vector3.Distance(transform.position, avoidee.transform.position) < avoidRange)
            {
                EscapeToSafePoint();
                
            }
        }
    }

    private void MaintainEyeContact()
    {
        Vector3 direction = (avoidee.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }

    private void EscapeToSafePoint()
    {
        var sampler = new PoissonDiscSampler(avoidRange * 2f, avoidRange * 2f, samplingRadius);
        List<Vector3> validPoints = new List<Vector3>();
        sampledPoints.Clear();

        foreach (var point in sampler.Samples())
        {
            Vector3 samplePoint = transform.position + new Vector3(point.x - avoidRange, 0, point.y - avoidRange);
            sampledPoints.Add(samplePoint);

            if (Vector3.Distance(samplePoint, avoidee.transform.position) > avoidRange)
            {
                // Check if the point is visible and not in the path
                if (IsPointVisible(samplePoint))
                {
                    validPoints.Add(samplePoint);
                }
            }

        }

        if (validPoints.Count > 0)
        {
            Vector3 closestPoint = GetClosestPoint(validPoints);
            agent.SetDestination(closestPoint);
        }
    }

    private bool IsPointVisible(Vector3 point)
    {
        Ray ray = new Ray(avoidee.transform.position, point - avoidee.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
             // The point is visible if the ray hits the avoidee or any object on the floor layer
            if (hit.collider.gameObject == avoidee || hit.collider.CompareTag("floor"))
            {
                return true; // Point is visible
            }
        }
        return false; // If no hit, it's visible
    }

    private Vector3 GetClosestPoint(List<Vector3> points)
    {
        Vector3 closestPoint = points[0];
        float closestDistance = Vector3.Distance(transform.position, closestPoint);

        foreach (var point in points)
        {
            float distance = Vector3.Distance(transform.position, point);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private void OnDrawGizmos()
    {
       if (!showGizmos) return;
       
       if (avoidee != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, avoidRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, avoidee.transform.position);
        }

        // Draw sampled points and lines
        foreach (var point in sampledPoints)
    {
        if (IsPointVisible(point))
        {
            Gizmos.color = Color.green; // Visible points
        }
        else
        {
            Gizmos.color = Color.black; // Hidden points
        }

        // Draw the point and line
        Gizmos.DrawSphere(point, 0.2f); // Draw the point
        Gizmos.DrawLine(transform.position, point); // Draw lines to the point
    }
    
    }

}