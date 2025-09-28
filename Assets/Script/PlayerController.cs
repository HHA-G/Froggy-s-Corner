using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float jumpPower;
    [SerializeField] private float radius;
    [SerializeField] GameObject bashableObject;
    [SerializeField] private float bashPower;
    [SerializeField] private float bashDuration;
    [SerializeField] GameObject Arrow;
    Vector3 BashDirection;
    private float BashTimeReset;
    private bool nearToBashable;
    private bool IsChoosingDir;
    private bool IsBashing;
    private bool canDash = true;
    private float dashTime = 0.2f;
    private float dashPower = 8f;
    private float dashCooldown = 1f;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRend;
    private float wallJumpCooldown;
    private HealthSystem healthSystem;
    public bool isDashing;
    AudioManager audioManager;

    private void Awake()
    {
        BashTimeReset = bashDuration;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal") * speed;

        if (healthSystem.isInvulnerable == true)
        {
            Physics2D.IgnoreLayerCollision(8, 9, true);
        }
        else if (healthSystem.isInvulnerable == false)
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
        }

        if (isDashing)
        {
            return;
        }

        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-4, 4, 4);
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            
            StartCoroutine(dash());
        }
        bash();

        //animation
        anim.SetBool("running", horizontalInput != 0 && isGrounded() && !isDashing);
        anim.SetBool("isGrounded", isGrounded());
        anim.SetBool("dashing", isDashing);
        anim.SetBool("invulnerable", healthSystem.isInvulnerable);
        anim.SetBool("hit", healthSystem.hit);
        anim.SetBool("onWall", onWall());


        //wall jump 
        if (wallJumpCooldown < 0.2f)
        {
            if (!isDashing && healthSystem.hit)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * speed / 2, 3);
            }

            if (onWall() && !isGrounded())
            {

                rb.gravityScale = 4;
                rb.velocity = Vector2.zero;
            }
            else if (!isDashing)
            {
                rb.gravityScale = 10;
            }
            else
            {
                rb.gravityScale = 0;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                //audioManager.PlaySFX(audioManager.jump);
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;

        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;

        }

    }

    private IEnumerator dash()
    {
        //audioManager.PlaySFX(audioManager.dash);
        canDash = false;
        isDashing = true;
        healthSystem.isInvulnerable = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        healthSystem.isInvulnerable = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    void FixedUpdate()
    {
        if (IsBashing == false && isDashing == false && healthSystem.hit == false)
        {
            rb.velocity = new Vector2(horizontalInput, rb.velocity.y);
        }
    }
    void bash()
    {
        RaycastHit2D[] rays = Physics2D.CircleCastAll(transform.position, radius, Vector3.forward);
        foreach (RaycastHit2D ray in rays)
        {

            nearToBashable = false;
            if (ray.collider.CompareTag("Enemy"))
            {
                nearToBashable = true;
                bashableObject = ray.collider.transform.gameObject;
                break;
            }
        }
        if (nearToBashable)
        {
            bashableObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Time.timeScale = 0;
                bashableObject.transform.localScale = new Vector2(1.4f, 1.4f);
                Arrow.SetActive(true);
                Arrow.transform.position = bashableObject.transform.position;
                IsChoosingDir = true;
            }
            else if (IsChoosingDir && Input.GetKeyUp(KeyCode.Mouse1))
            {
                Time.timeScale = 1;
                //audioManager.PlaySFX(audioManager.bash);
                bashableObject.transform.localScale = new Vector2(1f, 1f);
                IsChoosingDir = false;
                IsBashing = true;
                rb.velocity = Vector2.zero;
                transform.position = bashableObject.transform.position;
                BashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                BashDirection.z = 0;
                if (BashDirection.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                BashDirection = BashDirection.normalized;
                bashableObject.GetComponent<Rigidbody2D>().AddForce(-BashDirection * 10, ForceMode2D.Impulse);
                Arrow.SetActive(false);
            }
        }
        else if (bashableObject != null)
        {
            bashableObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        /// perform bash

        if (IsBashing)
        {
            if (bashDuration > 0)
            {
                bashDuration -= Time.deltaTime;
                rb.velocity = BashDirection * bashPower;
                Physics2D.IgnoreLayerCollision(8, 9, true);
                Destroy(bashableObject);
                bashableObject = null;
            }
            else
            {
                IsBashing = false;
                Physics2D.IgnoreLayerCollision(8, 9, false);
                bashDuration = BashTimeReset;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0), 0.01f, groundLayer | wallLayer);
        return hit.collider != null;
    }
    
}
