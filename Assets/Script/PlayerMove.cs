using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private float currentSpeed;

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float speedWalk;
    [SerializeField] private float speedRun;
    [SerializeField] private GroundChecker groundChecker;

    private float timer = 0;
    private bool isRun;
    private Vector2 direction;
    
    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpDuration = 1f;
    [SerializeField] float jumpCooldown = 0f;
    [SerializeField] float jumpMaxHeight = 2f;
    [SerializeField] float gravityMultiplier = 3f;
    [SerializeField] float jumpVelocity;

    private List<Timer> timers;
    private CountDownTimer jumpTimer;
    private CountDownTimer jumpCoolDownTimer;

    void Awake()
    {
        jumpTimer = new CountDownTimer(jumpDuration);
        jumpCoolDownTimer = new CountDownTimer(jumpDuration);
        
        timers = new List<Timer>(2){jumpTimer, jumpCoolDownTimer};
        
        jumpTimer.OnTimerStart += () => jumpTimer.Reset();
        jumpTimer.OnTimerStop += () => jumpCoolDownTimer.Reset();
    }

    void OnEnable()
    {
        _inputReader.Jump += OnJump;
    }
    
    void Start()
    {
        _inputReader.Enable();
    }

    void Update()
    {
        direction = new Vector2(_inputReader.Direction.x,  _inputReader.Direction.y).normalized;
        HandleTimer();
        if (direction != Vector2.zero)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                isRun = true;
            }
        }
        else
        {
            timer = 0f;
            isRun = false;
        }
    }

    private void FixedUpdate()
    {
        HanldeMovement();
        HanldeJump();
    }

    private void HanldeMovement()
    {
        if(direction.magnitude < 0.1f) return;
        currentSpeed = isRun ? speedRun : speedWalk;
        Vector2 movement = direction * currentSpeed * Time.deltaTime;
        rb.velocity = new Vector3(movement.x,rb.velocity.y,movement.y);
    }

    private void OnJump(bool value)
    {
        if (!value && !jumpTimer.IsRunning && !jumpCoolDownTimer.IsRunning && groundChecker.IsGround)
        {
            jumpTimer.Start();
        }
        else if (value && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }
    }

    private void HandleTimer()
    {
        foreach (var time in timers)
        {
            time.Tick(Time.deltaTime);
        }
    }
    
    const float point = 0.9f;
    private void HanldeJump()
    {
        if (!jumpTimer.IsRunning && groundChecker.IsGround)
        {
            jumpVelocity = 0f;
            return;
        }
        
        if (jumpTimer.IsRunning) 
        {
            if (jumpTimer.Progress >= point && groundChecker.IsGround)
            {
                // Vận tốc khi nhảy max
                jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpMaxHeight);
                Debug.Log("jgcsd");
            }
            else
            {
                jumpVelocity += (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                Debug.Log("jgcdsfsdfdssd");
            }
        }
        else
        {
            jumpVelocity = Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }
        
        rb.velocity = new Vector3(rb.velocity.x,jumpVelocity,rb.velocity.z);
    }
}