using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Joystick m_joystick;
    [SerializeField] float m_moveSpeed = 1f;
    [SerializeField] float m_jumpPower = 1f;
    Rigidbody2D m_rb;
    float m_lastHorizontalInput = 1f;
    bool m_isGroundedOnPlatform;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = (Input.GetAxisRaw("Horizontal") == 0f) ? m_joystick.Horizontal : Input.GetAxisRaw("Horizontal");
        m_rb.AddForce(Vector2.right * h * m_moveSpeed, ForceMode2D.Force);
        SwitchScale(h);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        float v = Input.GetAxisRaw("Vertical");

        if (v < 0)
        {
            GoDownThroughPlatform();
        }
    }

    void SwitchScale(float horizontalInput)
    {
        float x = m_lastHorizontalInput * horizontalInput;
        if (x != 0f)
        {
            if (x < 0)
            {
                transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            }
            m_lastHorizontalInput = horizontalInput;
        }
    }

    public void Jump()
    {
        m_rb.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);
    }

    public void GoDownThroughPlatform()
    {
        if (m_isGroundedOnPlatform)
        {
            StartCoroutine(GoDown());
        }
    }

    IEnumerator GoDown()
    {
        float targetY = transform.position.y - 1;
        Collider2D[] colliders;

        colliders = GetComponents<Collider2D>();

        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        while (transform.position.y > targetY)
        {
            transform.Translate(0, -0.01f, 0);
            yield return null;
        }

        foreach (var c in colliders)
        {
            c.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            m_isGroundedOnPlatform = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" && !m_isGroundedOnPlatform)
        {
            m_isGroundedOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            m_isGroundedOnPlatform = false;
        }
    }
}
