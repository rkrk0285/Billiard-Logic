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

    [Header("Current Parameters")]
    private float ballRadius;
    private float currentPower;
    private float epsilon;
    private int skipTurn;
    private Vector2 currVelocity;
    private E_MonsterState monsterState;

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
        currentPower = Power + PlayerPrefs.GetFloat("Power", 0);
        skipTurn = 0;
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
                monsterStat.ResetStartParameter();
                break;
        }
    }
    private void CheckBallState()
    {
        switch (monsterState)
        {
            case E_MonsterState.Ready:
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = worldPos - new Vector2(transform.position.x, transform.position.y);

                if (CheckAnyTurnToSkip())
                    break;                

                if (Input.GetMouseButtonDown(0))
                    MoveMonster(dir);                    
                else
                    DrawLine(dir);

                break;
            case E_MonsterState.Moving:
                if (rb.velocity.magnitude == 0)
                {
                    ResetEndPhysicsParameter();
                    ChangeState(E_MonsterState.Default);
                }
                break;
        }
    }
    protected void MoveMonster(Vector2 dir)
    {
        rb.velocity = dir.normalized * currentPower;
        rb.drag = DefaultDrag;

        monsterState = E_MonsterState.Moving;
        lr.enabled = false;
    }    
    private void DrawLine(Vector2 dir)
    {        
        RaycastHit2D hit = GetCircleCastHit(transform.position, dir, gameObject);        
        
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, hit.point + hit.normal * ballRadius);

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
    private bool CheckAnyTurnToSkip()
    {
        if (skipTurn > 0)
        {
            ChangeState(E_MonsterState.Default);
            skipTurn--;
            return true;
        }
        return false;
    }
    public IEnumerator DelayedStopBall()
    {
        yield return new WaitForFixedUpdate();
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(DelayedStopBall());
    }
    public void ResetEndPhysicsParameter()
    {
        currentPower = Power;
        rb.mass = Mass;
        rb.drag = Mathf.Max(DefaultDrag + PlayerPrefs.GetFloat("Drag", 0), 0);
    }
    public void SetSkipTurn(int count)
    {
        skipTurn = count;
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
