using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum E_MonsterState
{
    Default,
    Ready,
    Moving,
}

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(LineRenderer))]
public class MonsterController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float Power;
    [SerializeField] private float Mass;
    [SerializeField] private float DefaultDrag;
    [SerializeField] private float BeginDecelerateMagnitude;
    [SerializeField] private float DecelerateDrag;
    [SerializeField] private float StopMagnitude;
    [SerializeField] private float BreakDrag;

    [Space]
    [Range(0, 200)][SerializeField] private float ChangeDirectionSpeed;

    [Header("Current Parameters")]
    private float ballRadius;
    private float currPower;
    private float epsilon;    
    private Vector2 currVelocity;
    private Vector2 currDirection;
    private E_MonsterState monsterState;
    
    private float angleOffset;
    private bool angleIncrease;

    [Header("Components")]
    [SerializeField] private MonsterStat monsterStat;

    private Rigidbody2D rb;
    private BallStat ballStat;
    private LineRenderer lr;   
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        ballStat = GetComponent<BallStat>();

        rb.mass = Mass;
        rb.drag = Mathf.Max(DefaultDrag + PlayerPrefs.GetFloat("Drag", 0), 0);
        epsilon = rb.sharedMaterial.bounciness;

        ballRadius = transform.localScale.x / 2;
        currVelocity = rb.velocity;
        currDirection = Vector2.zero;
        currPower = Power + PlayerPrefs.GetFloat("Power", 0);        

        angleOffset = 0f;
        angleIncrease = true;
        
        monsterState = E_MonsterState.Default;
    }

    private void Update()
    {        
        DecelerateBall();
        CheckBallState();

        currVelocity = rb.velocity;
    }

    private void DecelerateBall()
    {
        if (rb.velocity.magnitude < StopMagnitude)
            rb.velocity = Vector2.zero;
        else if (rb.velocity.magnitude < BeginDecelerateMagnitude)
            rb.drag = DecelerateDrag;
    }
    public void ChangeState(E_MonsterState state)
    {
        monsterState = state;
        switch (monsterState)
        {
            case E_MonsterState.Ready:
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currDirection = worldPos - new Vector2(transform.position.x, transform.position.y);
                monsterStat.ResetStartParameter();
                break;
        }
    }
    private void CheckBallState()
    {
        if (monsterState == E_MonsterState.Ready)
        {
            float moveY = 0f;
            float moveX = 0f;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveY = 1f;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveY = -1f;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveX = -1f;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveX += 1f;
            }

            transform.Translate(new Vector3(moveX, moveY, 0f) * 0.005f);
        }
    }
    protected void MoveMonster(Vector2 dir)
    {
        rb.velocity = dir.normalized * currPower;
        rb.drag = DefaultDrag;        
        
        monsterState = E_MonsterState.Moving;        

        angleOffset = 0f;
        angleIncrease = true;
        lr.enabled = false;
    }
    private Vector2 ChangeAngleByDirection(Vector2 dir)
    {                    
        if (angleIncrease)
        {
            angleOffset += ChangeDirectionSpeed * Time.deltaTime;
            if (angleOffset > 45f)
            {
                angleOffset = 45f;
                angleIncrease = false;
            }
        }
        else
        {
            angleOffset -= ChangeDirectionSpeed * Time.deltaTime;
            if (angleOffset < -45f)
            {
                angleOffset = -45f;
                angleIncrease = true;
            }
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;
        float radian = angle * Mathf.Deg2Rad;
        return new Vector2 (Mathf.Cos(radian), Mathf.Sin(radian));       
    }
    private static float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0)
            angle += 360f;
        return angle;
    }
    private void DrawLine(Vector2 dir)
    {        
        RaycastHit2D hit = GetCircleCastHit(transform.position, dir, gameObject);

        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + (Vector3)dir.normalized * 5f);
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.enabled = true;
    }
    protected RaycastHit2D GetCircleCastHit(Vector2 pos, Vector2 dir, GameObject obj)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(pos, ballRadius, dir, 100f);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject != obj)
                return hit[i];
        }
        return new RaycastHit2D();
    }    
    public IEnumerator DelayedStopBall()
    {
        yield return new WaitForFixedUpdate();
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(DelayedStopBall());
    }
    public void ResetEndPhysicsParameter()
    {
        currPower = Power;
        
        angleOffset = 0f;
        angleIncrease = true;

        rb.mass = Mass;
        rb.drag = Mathf.Max(DefaultDrag + PlayerPrefs.GetFloat("Drag", 0), 0);
    }    
    public Vector2 GetCurrVelocity()
    {
        return currVelocity;
    }
    public E_MonsterState GetMonsterState()
    {
        return monsterState;
    }    
    public void BreakMonster()
    {
        rb.drag = BreakDrag;
    }

}
