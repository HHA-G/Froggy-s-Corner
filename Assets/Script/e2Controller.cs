using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e2Controller : MonoBehaviour
{
    public GameObject player;
    public float jumpPower;
    public float speed;
    private float distance;
    private float jumpCooldown;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 directon = player.transform.position - transform.position;

        if (distance < 18)
        {
            if (directon.x > 0)
            {
                transform.localScale = new Vector3(4, 4, 4);
            }
            else if (directon.x < 0)
            {
                transform.localScale = new Vector3(-4, 4, 4);
            }
            if (jumpCooldown == 1)
            {
                jumpCooldown = 0;
                if (isGrounded())
                {
                    jump();
                }
            }
            else
            {
                jumpCooldown += Time.deltaTime;
                if (jumpCooldown > 1)
                {
                    jumpCooldown = 1;
                }
            }
        }
    }
    private void jump()
    {
        rb.velocity = new Vector2(transform.localScale.x * speed, jumpPower);
    }
    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

}
