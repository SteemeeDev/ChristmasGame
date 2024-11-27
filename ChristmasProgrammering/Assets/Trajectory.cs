using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public void PredictTrajectory(Vector3 velocity, Vector3 position, int maxPoints)
    {
        UpdateLineRenderer(maxPoints, 0, position);

        for (int i = 1; i < maxPoints; i++)
        {
            velocity = CalculateNewVelocity(velocity, 0.03f);
            Vector3 nextPosition = position + velocity * 0.03f;

            if (nextPosition.magnitude > 30)
            {
                break;
            }

            position = nextPosition;
            UpdateLineRenderer(i+1, i, position);
        }
    }
    private Vector3 CalculateNewVelocity(Vector3 velocity, float increment)
    {
        velocity += Physics.gravity * increment;
        return velocity;
    }

    private void UpdateLineRenderer(int maxPoints, int point, Vector3 pointPos)
    {
        lineRenderer.positionCount = maxPoints;
        lineRenderer.SetPosition(point, pointPos);
    }
}
