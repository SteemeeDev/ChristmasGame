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
    [SerializeField] Transform shooter;

    [SerializeField] GameObject wireGrab1;
    [SerializeField] GameObject wireGrab2;
    [SerializeField] GameObject wireGrab3;
    [SerializeField] Transform end1;
    [SerializeField] Transform end2;

    [SerializeField] LineRenderer wire;

    [SerializeField] LayerMask[] layerMask;
    [SerializeField, Range(0f,1f)] float turnAmount;
    void Awake()
    {
        trajectory = GetComponent<Trajectory>();
    }

    RaycastHit hit;
    Vector3 exitPos;
    Quaternion shooterExitRotation;
    Vector3 Velocity;
    bool drag = false;
    bool shoot = false;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        wire.SetPosition(1, wireGrab1.transform.position);
        wire.SetPosition(2, wireGrab2.transform.position);
        wire.SetPosition(3, wireGrab3.transform.position);
        wire.SetPosition(0, end1.position);
        wire.SetPosition(4, end2.position);

        if (Input.GetMouseButton(0))
        {
            //Hit the hitbox
            if (Physics.Raycast(ray, 5, layerMask[0]))
            {
                drag = true;
            }
            //Dragging the object
            if (Physics.Raycast(ray, out hit, 5, layerMask[1]) && drag)
            {
                shoot = true;

                StopCoroutine(Slerp());

                point.gameObject.SetActive(true);
                point.transform.position = hit.point;
                // Get the vector between the ball and the middle of the shooter

                Velocity = (shootPoint.position - point.transform.position) * 20f;
                Velocity.y = Mathf.Clamp(Velocity.y, 1, float.MaxValue); 
                // Up the y component to hit upper areas easier
                Velocity.y = Mathf.Pow(Velocity.y, 1.3f);

                trajectory.lineRenderer.enabled = true;
                trajectory.PredictTrajectory(Velocity, shootPoint.position, 65);


                shooter.rotation = Quaternion.LookRotation(Vector3.Lerp((shootPoint.position - point.transform.position), Vector3.forward, turnAmount), point.transform.up);
            }
        }
        else if (Input.GetMouseButtonUp(0) && shoot)
        {
            shoot = false;
            drag = false;
            //trajectory.hitMarker.gameObject.SetActive(false);
            trajectory.lineRenderer.enabled = false;
            shooterExitRotation = shooter.rotation;
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
            easedT = 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2)); // A circle like easing function

            point.transform.position = Vector3.Lerp(exitPos, shootPoint.position, easedT);
            shooter.rotation = Quaternion.Lerp(shooterExitRotation, Quaternion.identity, easedT);


            //Prefire the ball so that the transition
            //between the fake ball and the new ball is more seamless
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
        // Ensure that the objects return to their defualt state
        shooter.rotation = Quaternion.identity;
        point.transform.position = shootPoint.position;
    }
}
