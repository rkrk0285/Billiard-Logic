using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnWallSkill : SkillBase
{
    [SerializeField] private GameObject WallPrefab;    

    private GameObject wall;    
    private GameObject mouseIndicator;
    private bool activeSkill = false;

    protected override void Start()
    {
        wall = null;
        mouseIndicator = null;
    }
    public override void Activate()
    {
        base.Activate();
        Debug.Log(this.gameObject.name + " 벽 스폰 스킬 발동");        
        activeSkill = true;
    }

    private void Update()
    {
        if (activeSkill)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);
            if (Input.GetMouseButtonDown(0))
            {
                activeSkill = false;
                SpawnWall(dir, worldPos);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                activeSkill = false;
                CancelMouseIndicator();
            }
            else
            {
                ShowWall(dir, worldPos);
            }
        }
    }

    private void SpawnWall(Vector2 dir, Vector2 pos)
    {
        wall = Instantiate(WallPrefab, mouseIndicator.transform.position, Quaternion.identity);
        wall.transform.up = -dir;

        CancelMouseIndicator();
    }
    private void CancelMouseIndicator()
    {
        Destroy(mouseIndicator);
        mouseIndicator = null;
    }
    private void ShowWall(Vector2 dir, Vector2 pos)
    {
        if (mouseIndicator == null)
        {
            mouseIndicator = Instantiate(WallPrefab, pos, Quaternion.identity);
            mouseIndicator.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        
        if (Vector2.Distance(pos, transform.position) < 3f)
            mouseIndicator.transform.position = pos;
        else
            mouseIndicator.transform.position = transform.position + new Vector3(dir.normalized.x, dir.normalized.y, 0) * 3;

        mouseIndicator.transform.up = -dir;
    }
}
