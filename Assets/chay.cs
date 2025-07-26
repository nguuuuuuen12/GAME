using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

public class chay : MonoBehaviour
{
    [SerializeField] private float movespeed = 5f;//tốc độ chạy
    private float jumpPower = 30f;//sức nhảy
    [SerializeField] private bool isfacingright  = true;//kt
    [SerializeField] private Transform groundcheck;//check có chạm đất
    
    [SerializeField] private LayerMask groundLayer;//mặt đất

    private float Horizontal;
    private bool ground = true;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool canDash = true;
    private bool isdashing;
    private float dashingPower = 24f;
    private float dashingtime = 0.2f;
    private float dashingcooldown = 1f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
           //kiểm tra dash
        if (isdashing)
        {
            return;
        }
        Horizontal = Input.GetAxisRaw("Horizontal");
        if (Horizontal != 0)
        {
            animator.SetBool("isplayerrun", true);
        }
        else
        {
            animator.SetBool("isplayerrun", false);
        }
        if (Input.GetButtonDown("Jump") && isgrounded())
        {
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isplayerjump", true);
        }
        else if (isgrounded())
        {
            animator.SetBool("isplayerjump", false);
        }
        if (Input.GetButtonDown("Jump") && rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        flip();
        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
    }
    private void FixedUpdate()
    {
        if (isdashing)
        {
            return;
        }
        rb.linearVelocity = new Vector2(Horizontal * movespeed, rb.linearVelocity.y);
    }
    private void flip()
    {
        if (isfacingright && Horizontal < 0f || !isfacingright && Horizontal > 0f)
        {
            isfacingright = !isfacingright;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }
    private bool isgrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundLayer);
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isdashing = true;
        float origianlgravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingtime);
        isdashing = false;
        yield return new WaitForSeconds(dashingcooldown);
        canDash = true;
    }
}
