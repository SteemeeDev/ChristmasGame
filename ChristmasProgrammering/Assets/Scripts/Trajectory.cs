using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Trajectory : MonoBehaviour
{
    [SerializeField] LayerMask[] hitLayers;

    public LineRenderer lineRenderer;
    public Transform hitMarker;

    RaycastHit hit;

    public void PredictTrajectory(Vector3 velocity, Vector3 position, int maxPoints)
    {
        UpdateLineRenderer(maxPoints, 0, position);

        for (int i = 1; i < maxPoints; i++)
        {
            velocity = CalculateNewVelocity(velocity, 0.03f);
            Vector3 nextPosition = position + velocity * 0.03f;

            if (Physics.Raycast(position, nextPosition, out hit, (nextPosition-position).magnitude, hitLayers[0]))
            {
                MoveHitMarker(hit);
                break;
            }

            if (Physics.Raycast(position, nextPosition, out hit, (nextPosition - position).magnitude, hitLayers[1]))
            {
                MoveHitMarker(hit);
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

    private void MoveHitMarker(RaycastHit hit)
    {
        hitMarker.gameObject.SetActive(true);

        // Offset marker from surface
        float offset = 0.025f;
        hitMarker.position = hit.point + hit.normal * offset;
       // hitMarker.rotation = Quaternion.Euler(hit.normal);
    }

}
