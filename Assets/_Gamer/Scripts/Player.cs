using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 20;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;


    private bool isGrounded = true;
    private bool isJumping = true;
    private bool isAttack = false;

    public float horizontal;

    private float jumpForce = 500;
    private int coin = 0;
    private Vector3 savePoint;


    // Awake duoc goi khi bat dau cua obj trc ca Start
    private void Awake()
    {
        coin = PlayerPrefs.GetInt("Coin", coin);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounder();
        //-1 -> 0 -> 1
        //horizontal = Input.GetAxis("Horizontal");
        // verticle = Input.GetAxis("Verticle");

        if(isGrounded)
        {
            if (IsDead)
            {
                return;
            }

            if(isAttack)
            {
                
                return;
            }
            if (isJumping)
            {
                return;
            }

            // defaul
            if (isGrounded && Math.Abs(horizontal) <= 0.1f)
            {
                rb.velocity = Vector2.zero;
                ChangeAnim("idle");
                Debug.Log("Amin Idle");
            }

            if (Math.Abs(horizontal) > 0.8f)
            {
                
                ChangeAnim("run");
            }
            //else ChangeAnim("idle");

            // jump
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
            {
                Jump();           
            }
                       

            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Throw();
            }
        }

        
        // check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        // run left right
        if (Math.Abs(horizontal) > 0.5f)
        {
            rb.velocity = new Vector2 (horizontal * speed,rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal < 0? 180 : 0, 0)); 
        }
        /*// defaul
        if(isGrounded && Math.Abs(horizontal)<=0.1f)
        {
            rb.velocity = Vector2.zero;
            ChangeAnim("idle");
            Debug.Log("Amin Idle");
        }*/

    }// end update();

    // save init value
    public override void OnInit()
    {
        base.OnInit();        
        isAttack = false;        
        transform.position = savePoint;
        SavePoint();
        DeActiveAttack();
        UIManager.instance.SetCoin(coin);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Invoke(nameof(OnInit),1f);
    }
    // check on the grouder
    private bool CheckGrounder()
    {
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        
        /*if(hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }*/
        return hit.collider != null;
    }

    public void Attack()
    {
        if (!isAttack)
        {
            rb.velocity = Vector2.zero;
            ChangeAnim("attack");
            Debug.Log("attack");
            isAttack = true;
            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f);
            Invoke(nameof(ResetAttack), 0.4f);
        }
        else return;
        
    }

    public void Throw()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.4f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            isJumping = true;
            ChangeAnim("jump");
            rb.AddForce(jumpForce * Vector2.up);
        }
        else return;
        
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack= false;
    }

    // change animation

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
        Debug.Log(horizontal);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);
            UIManager.instance.SetCoin(coin);
            PlayerPrefs.SetInt("Coin", coin);
        }

        if(collision.tag == "DeathZone")
        {
            //ChangeAnim("dead");
            OnHit(100f);
            //Invoke("OnInit", 1f);
        }
    }

  
}
