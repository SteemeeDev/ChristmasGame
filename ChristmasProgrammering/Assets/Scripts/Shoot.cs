using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
public class Shoot : MonoBehaviour
{
    Trajectory trajectory;

    [SerializeField] GameObject point;

    [SerializeField] GameObject grabHitBox;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject wireGrab1;
    [SerializeField] GameObject wireGrab2;
    [SerializeField] GameObject wireGrab3;
    [SerializeField] LineRenderer wire;

    [SerializeField] LayerMask[] layerMask;
    void Start()
    {
        trajectory = GetComponent<Trajectory>();
    }

    RaycastHit hit;
    Vector3 exitPos;
    Vector3 Velocity;
    bool drag = false;
    bool shoot = false;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, 5, layerMask[0]))
            {
                drag = true;
            }
            if (Physics.Raycast(ray, out hit, 5, layerMask[1]) && drag)
            {
                shoot = true;

                StopCoroutine(Slerp());

                point.gameObject.SetActive(true);
                point.transform.position = hit.point;
                Velocity = (shootPoint.position - point.transform.position) * 20f;
                Velocity.y = Mathf.Clamp(Velocity.y, 1, float.MaxValue);
                Velocity.y = Mathf.Pow(Velocity.y, 1.3f);

                trajectory.lineRenderer.enabled = true;
                trajectory.PredictTrajectory(Velocity, shootPoint.position, 65);

                wire.SetPosition(1, wireGrab1.transform.position);
                wire.SetPosition(2, wireGrab2.transform.position);
                wire.SetPosition(3, wireGrab3.transform.position);


            }
        }
        else if (Input.GetMouseButtonUp(0) && shoot)
        {
            shoot = false;
            drag = false;
            //trajectory.hitMarker.gameObject.SetActive(false);
            trajectory.lineRenderer.enabled = false;
            exitPos = point.transform.position;
            StartCoroutine(Slerp());
        }

        //Debug.DrawRay(exitPos, shootPoint.position-exitPos, Color.magenta);
    }

    IEnumerator Slerp()
    {

        float elapsedTime = 0;
        float targetTime = (shootPoint.position - exitPos).magnitude*0.08f;

        GameObject ball = Instantiate(ballPrefab);
        ball.GetComponent<Rigidbody>().useGravity = false;

        bool a = true;

        float t = 0.02f;
        float easedT = 0;

        while (elapsedTime < targetTime)
        {
            elapsedTime = elapsedTime + Time.deltaTime;

            t = elapsedTime / targetTime;
            t = Mathf.Clamp01(t);
            easedT = 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2));

            point.transform.position = Vector3.Lerp(exitPos, shootPoint.position, easedT);

            wire.SetPosition(1, wireGrab1.transform.position);
            wire.SetPosition(2, wireGrab2.transform.position);
            wire.SetPosition(3, wireGrab3.transform.position);

            if (t > 0.9f && a)
            {
                ball.GetComponent<Rigidbody>().useGravity = true;
                ball.GetComponent<Rigidbody>().velocity = Velocity;
                a = false;
                ball.transform.position = shootPoint.position;
                point.SetActive(false);
                ball.GetComponent<MeshRenderer>().enabled = true;
            }

            yield return null; 
        }

        point.transform.position = shootPoint.position;
        wire.SetPosition(1, wireGrab1.transform.position);
        wire.SetPosition(2, wireGrab2.transform.position);
        wire.SetPosition(3, wireGrab3.transform.position);
    }
}
