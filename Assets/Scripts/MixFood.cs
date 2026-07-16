
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MixFood : MonoBehaviour
//miau

{
    [SerializeField] private List<RectTransform> waypoints;
    [SerializeField] private float moveSpeed = 500f;
    [SerializeField] private float arrivalDistance = 5f;

    private RectTransform rectTransform;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        print("Active");
    }

    public void AdvanceToNextWaypoint()
    {

        print("Test");
        // Only advance if not moving and there are waypoints left
        if (!isMoving)
        {
            StartCoroutine(MoveToWaypoint(currentWaypointIndex));
            currentWaypointIndex++;
        }
    }

    private IEnumerator MoveToWaypoint(int index)
    {

        print("StrtMoving");
        isMoving = true;
        Vector2 targetPos = waypoints[index].anchoredPosition;

        while (Vector2.Distance(rectTransform.anchoredPosition, targetPos) > arrivalDistance)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(
                rectTransform.anchoredPosition,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
        isMoving = false;
    }

    // Optional: reset spoon to first waypoint and restart sequence
    public void ResetSpoon()
    {
        if (waypoints.Count > 0)
        {
            rectTransform.anchoredPosition = waypoints[0].anchoredPosition;
            currentWaypointIndex = 1; // Next click moves to waypoint 1
            StopAllCoroutines();
            isMoving = false;
        }
    }
}