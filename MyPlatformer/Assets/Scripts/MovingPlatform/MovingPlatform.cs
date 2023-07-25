using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private WaypointPath _waypointPath;

    [SerializeField]
    private float _speed;

    private int _targetWaypointIndex;

    private Transform _previousWaypoint;
    private Transform _targetWaypoint;

    private float _timeToWaypoint;//time it will take to get to the next waypoint
    private float _elapsedTime;


    private void OnDrawGizmosSelected()
    {
        _waypointPath.DrawWaypointGizmos();
    }

    // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
        float elapsedPercentage = _elapsedTime / _timeToWaypoint;//gets the percentage of the platforms journey to the next waypoint

        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);//slows down platform whenever it reaches next platform

        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);//platform movement
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);//rotates platform to match waypoints rotation

        if (elapsedPercentage >= 1)//checks if platform has reached its target waypoint
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);//sets current waypoint as new previous waypoint
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);//targets next waypoint
        _targetWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);//sets current waupoint as next waypoint

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);//uses current waypoint and next waypoint to travel to in order to find the distance
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    
}
