using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] public GameObject pointA;
    [SerializeField] public GameObject pointB;
    [SerializeField] public GameObject pointC;
    [SerializeField] public GameObject pointD;
    [SerializeField] public GameObject pointE;
    [SerializeField] public GameObject pointF;
    private Rigidbody2D rb;
    private Transform currentPoint;
    [SerializeField] public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointA.transform;
        Physics2D.IgnoreLayerCollision(10, 6, true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointA.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(0, -speed);
        }
        else if (currentPoint == pointC.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else if (currentPoint == pointD.transform)
        {
            rb.velocity = new Vector2(0, speed);
        }
        else if (currentPoint == pointE.transform)
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        else if (currentPoint == pointF.transform)
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointC.transform;
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointC.transform)
        {
            currentPoint = pointD.transform;
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointD.transform)
        {
            currentPoint = pointE.transform;
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointE.transform)
        {
            currentPoint = pointF.transform;
        }
    }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointC.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointD.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointE.transform.position, 0.5f);
        }
}
