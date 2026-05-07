using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MixFood : MonoBehaviour//,IBeginDragHandler, IEndDragHandler
{

    [SerializeField] private List<Transform> waypoints;  // Drag your transforms here
    [SerializeField] private float moveSpeed = 5f;       // Units per second
    [SerializeField] private float arrivalDistance = 0.05f; // How close to consider "reached"

    private bool isMoving = false;
    private int currentWaypointIndex = 0;

    void Update()
    {
        // Any mouse click or touch tap
        if (Input.GetMouseButtonDown(0))
        {
            if (!isMoving)
            {
                StartCoroutine(MoveThroughWaypoints());
            }
        }
    }

    IEnumerator MoveThroughWaypoints()
    {
        isMoving = true;
        currentWaypointIndex = 0;

        while (currentWaypointIndex < waypoints.Count)
        {
            Transform target = waypoints[currentWaypointIndex];

            // Move towards the target
            while (Vector3.Distance(transform.position, target.position) > arrivalDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Snap to exact position (optional)
            transform.position = target.position;

            currentWaypointIndex++;
        }

        isMoving = false;
    }
}
