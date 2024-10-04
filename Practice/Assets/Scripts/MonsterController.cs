using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BallState
{
    Default,
    Ready,
    Attacking,
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

    [Header("Current Parameters")]
    private float currentPower;
    private float epsilon;
    private Vector2 currVelocity;
    private E_BallState ballState;

    [Header("Components")]
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

        currVelocity = rb.velocity;
        currentPower = Power + PlayerPrefs.GetFloat("Power", 0);
        ballState = E_BallState.Default;
    }

    private void Update()
    {
        DecelerateBall();
    }

    private void DecelerateBall()
    {
        if (rb.velocity.magnitude < StopMagnitude)
            rb.velocity = Vector2.zero;
        else if (rb.velocity.magnitude < BeginDecelerateMagnitude)
            rb.drag = DecelerateDrag;
    }
}
