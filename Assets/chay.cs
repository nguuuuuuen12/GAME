using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net;

public class chay : MonoBehaviour
{
    [SerializeField] private float movespeed = 10f;//tốc độ chạy
    private float jumpPower = 25f;//sức nhảy
    [SerializeField] private bool isfacingright  = true;//kt
    [SerializeField] private Transform groundcheck;//check có chạm đất
    
    [SerializeField] private LayerMask groundLayer;//mặt đất

    private float Horizontal;
    private float Vertical;
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
        Vertical = Input.GetAxisRaw("Vertical");
        if (Horizontal != 0 || Vertical!=0)
        {
            animator.SetBool("isplayerrun", true);
        }
        else
        {
            animator.SetBool("isplayerrun", false);
        }
        flip();
        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
    }
    /// </summary>
    private void FixedUpdate()
    {
        if (isdashing)
        {
            return;
        }
        rb.linearVelocity = new Vector2(Horizontal, Vertical);
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
    private IEnumerator Dash()
    {
        
        if (Horizontal != 0)
        {
            canDash = false;
            isdashing = true;
            animator.SetBool("playerroll", true);
            float origianlgravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            yield return new WaitForSeconds(dashingtime);
            rb.gravityScale = origianlgravity;
            isdashing = false;
            animator.SetBool("playerroll", false);
            yield return new WaitForSeconds(dashingcooldown);
            canDash = true;
        }
    }
}
