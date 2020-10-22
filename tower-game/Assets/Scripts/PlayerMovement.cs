using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float dragPower = 10f;
    public float maxDrag = 5f;

    public Rigidbody2D rb;
    public LineRenderer lr;

    private Vector3 dragStartPos;
    private Vector3 posZero;
    private Touch touch;

    private bool canMove = true;

    private void Start()
    {
        posZero.Set(-10, -10, 0);
        lr.SetPosition(0, posZero);
        lr.SetPosition(1, posZero);
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if(canMove == true)
                    DragStart();
                    StartCoroutine(Czekaj());
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (canMove == true)
                    Dragging();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (canMove == true)
                    DragEnd();
                
            }

        }
    }

    void DragStart()
    {
        dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragStartPos.z = 0f;
        lr.positionCount = 1;
        lr.SetPosition(0, dragStartPos);
    }
    void Dragging()
    {
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        draggingPos.z = 0f;
        lr.positionCount = 2;
        lr.SetPosition(1, draggingPos);
    }
    void DragEnd()
    {
        lr.positionCount = 0;
        Vector3 dragEndPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragEndPos.z = 0f;

        Vector3 force = dragStartPos - dragEndPos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * dragPower;

        rb.AddForce(clampedForce, ForceMode2D.Impulse);
    }

    IEnumerator Czekaj()
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
