using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;

    private string currentAnimName;
    private float hp;

    public bool IsDead => hp <= 0;
    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100f;
        healthBar.OnInit(100f, transform);
        healthBar.SetNewHp(hp);
    }

    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("dead");
        Invoke(nameof(OnDespawn), 2f);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
        else return;

    }

    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            Instantiate(combatTextPrefab, transform.position + Vector3.up * 2f,Quaternion.identity).OnInit(damage);
            
            
            if (IsDead) 
            {
                hp = 0;
                OnDeath();                
            }            
            healthBar.SetNewHp(hp);
        }
    }

    
}
