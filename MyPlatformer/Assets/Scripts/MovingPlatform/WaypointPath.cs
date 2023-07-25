using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1;

        if (nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }

        return nextWaypointIndex;
    }

    public int GetFinalWaypoint()
    {
        return transform.childCount;
    }

    private void OnDrawGizmos()
    {
        if (IsWaypointSelected())//will only display gizmos for waypoints if any of the parent or child gameobjects are selected
        {
            DrawWaypointGizmos();
        }


    }

    public void DrawWaypointGizmos()
    {
        for (int waypointIndex = 0; waypointIndex < transform.childCount; waypointIndex++)
        {
            Transform waypoint = GetWaypoint(waypointIndex);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(waypoint.position, 0.6f);

            int nextWaypointIndex = GetNextWaypointIndex(waypointIndex);
            Transform nextWaypoint = GetWaypoint(nextWaypointIndex);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(waypoint.position, nextWaypoint.position);
        }
    }

    private bool IsWaypointSelected()
    {
        if (Selection.transforms.Contains(transform))
        {
            return true;
        }

        foreach(Transform child in transform)
        {
            if (Selection.transforms.Contains(child))
            {
                return true;
            }
        }

        return false;
    }
}
