using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject breakEffect;

    GameObject particle;
    float elapsedTime = 0;
    private void Start()
    {
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 5f || particle != null && particle.GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        particle = Instantiate(breakEffect, transform);
        particle.transform.position = collision.GetContact(0).point;
        particle.transform.rotation = Quaternion.Euler(new Vector3(-90, 0 ,0));

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
