using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleController : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 5f;
    public Rigidbody2D rb;

    Vector2 moveDirection = Vector2.zero;
    Vector2 mousePosition = Vector2.zero;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2 (moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x*moveSpeed, moveDirection.y*moveSpeed);
    }
}
