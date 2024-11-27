using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
public class Shoot : MonoBehaviour
{
    Trajectory trajectory;

    [SerializeField] GameObject point;
    [SerializeField] Transform shootPoint;
    [SerializeField] LineRenderer wire;

    [SerializeField] LayerMask layerMask;

    Rigidbody pointRB;
    void Start()
    {
        trajectory = GetComponent<Trajectory>();
        pointRB = point.GetComponent<Rigidbody>();
    }

    RaycastHit hit;
    Vector3 exitPos;

    Vector3 Velocity;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, 5, layerMask))
            {
                StopCoroutine(Slerp());
                pointRB.isKinematic = false;
                pointRB.useGravity = false;
                pointRB.velocity = Vector3.zero;
                pointRB.angularVelocity = Vector3.zero;
                point.transform.position = hit.point;
                Velocity = (shootPoint.position - point.transform.position) * 20f;
                Velocity.y = Mathf.Pow(Velocity.y,1.3f);
                trajectory.lineRenderer.enabled = true;
                trajectory.PredictTrajectory(Velocity, point.transform.position, 20);

                wire.SetPosition(1, point.transform.position);
                //Debug.DrawRay(shootPoint.position, Velocity, Color.magenta);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            trajectory.lineRenderer.enabled = false;
            exitPos = point.transform.position;
            StartCoroutine(Slerp());
        }

        //Debug.DrawRay(exitPos, shootPoint.position-exitPos, Color.magenta);
    }

    IEnumerator Slerp()
    {

        float elapsedTime = 0;
        float targetTime = shootPoint.position.magnitude - exitPos.magnitude;
        //Debug.Log(targetTime);

        while (elapsedTime < targetTime)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            point.transform.position = Vector3.Lerp(exitPos, shootPoint.position,Mathf.SmoothStep(0, 2, elapsedTime / targetTime));
            wire.SetPosition(1, point.transform.position);
            yield return null; 
        }
        //pointRB.isKinematic = true;
        pointRB.useGravity = true;
        wire.SetPosition(1, shootPoint.position);
        pointRB.velocity = Velocity;
    }
}
