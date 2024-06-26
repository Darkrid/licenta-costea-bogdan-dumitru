using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundScript : MonoBehaviour
{
    private Transform target;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float roundSpeed = 5f;
    [SerializeField] private int roundDamage = 1;

    Vector3 m_EulerAngleVelocity = new Vector3(0, 100, 0);

    public void SetTarget(Transform _target)
    {
        target = _target;
    } 

    private void Start()
    {
        if (!target) return;

        Vector3 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * roundSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Health>().TakeDamage(roundDamage);
        Destroy(gameObject);
    }
}