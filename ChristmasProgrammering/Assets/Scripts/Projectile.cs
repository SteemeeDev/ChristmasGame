using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject breakEffect;
    public AudioSource hitSound;

    GameObject particle;
    float elapsedTime = 0;
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 5f )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        particle = Instantiate(breakEffect);
        particle.transform.position = other.ClosestPoint(transform.position);
        particle.transform.rotation = Quaternion.Euler(new Vector3(-90, 0 ,0));

        hitSound.GetComponent<PlayRandom>().m_PlayRandom();

        Destroy(gameObject);
    }
}
