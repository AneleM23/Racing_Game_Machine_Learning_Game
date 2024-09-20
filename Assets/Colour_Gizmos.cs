using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colour_Gizmos : MonoBehaviour
{

    public Transform[] FarRightWaypoints;
    public Transform[] middleLaneWaypoints;
    public Transform[] FarLeftWaypoints;


    // Draw Gizmos to visualize the waypoints
    private void OnDrawGizmos()
    {
        // Draw inner lane waypoints in red
        DrawWaypointsGizmos(FarRightWaypoints, Color.red);

        // Draw middle lane waypoints in green
        DrawWaypointsGizmos(middleLaneWaypoints, Color.green);

        // Draw outer lane waypoints in blue
        DrawWaypointsGizmos(FarLeftWaypoints, Color.blue);
    }

    private void DrawWaypointsGizmos(Transform[] waypoints, Color gizmoColor)
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = gizmoColor;

        for (int i = 0; i < waypoints.Length; i++)
        {
            // Draw the waypoints as spheres
            Gizmos.DrawSphere(waypoints[i].position, 1f);

            // Draw lines connecting the waypoints
            if (i > 0)
            {
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }
        }
    }

}
