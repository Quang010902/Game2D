using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imageFill;
    [SerializeField] Vector3 offset;

    float hp;
    float maxHp;

    private Transform target;

    // Update is called once per frame
    void Update()
    {
        if (imageFill != null)
        {
            imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHp, Time.deltaTime * 5f);
        }
        else Debug.Log("imagefill loi");
        if (target != null) 
        {
            transform.position = target.position + offset;
        }
        else Debug.Log("target loi");


    }

    public void OnInit(float maxHp, Transform target)
    {
        this.target = target;
        
        this.maxHp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }
}
