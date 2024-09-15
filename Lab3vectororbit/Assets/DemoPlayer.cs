using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DemoPlayer : MonoBehaviour
{
  private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int IsRunning = Animator.StringToHash("Run");

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Animator _animator;
    private Vector2 _movement;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        bool isMoving = _movement.sqrMagnitude > 0;
        _animator.SetBool(IsRunning, isMoving);  
        _animator.SetFloat(Horizontal, _movement.x);
        _animator.SetFloat(Vertical, _movement.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + _movement * moveSpeed * Time.fixedDeltaTime);
    }
}
