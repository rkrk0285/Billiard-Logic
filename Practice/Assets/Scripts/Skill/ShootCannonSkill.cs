using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCannonSkill : SkillBase
{           
    [SerializeField] private GameObject CannonBall;

    private bool activeSkill = false;

    private void Update()
    {
        if (activeSkill)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);

            if (Input.GetMouseButtonDown(0))
            {
                activeSkill = false;
                ShootCannon(dir);
            }
            else            
                DrawLine(dir);            
        }
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " 스킬 발동");        
        activeSkill = true;
    }    

    private void ShootCannon(Vector2 dir)
    {        
        GameObject clone = Instantiate(CannonBall, transform.position, Quaternion.identity, transform);
        clone.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 2000);
        Destroy(clone, 5f);
    }
}
