

using UnityEngine;
using System.Collections.Generic;

public class MoveTo : MonoBehaviour
{
    public List<Vector3> points;
    public int index = 0;
    public float moveSpeed = .5f;
    public AnimationCurve curve;

    private bool isMoving;
    private Vector3 origin;
    private float timer = 0f;
    private RectTransform rectTransform;
	
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

	void Update ()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;

            float value = timer / moveSpeed;
            if(value <= 1f)
            {
                Move(value);
            }
            else
            {
                value = 1f;
                Move(value);
                isMoving = false;
            }
        }
	}

    public void Next()
    {
        if (index + 1 == points.Count)
            index = 0;
        else
            index++;
        MoveToPoint();
    }

    public void Prev()
    {
        if (index == 0)
            index = points.Count - 1;
        else
            index--;
        MoveToPoint();
    }

    public void ScrollValue(float value)
    {
        if (value > 0f)
            Prev();
        else if (value < 0f)
            Next();
    }

    public void AtIndex(int index)
    {
        this.index = index;
        MoveToPoint();
    }

    public void MoveToPoint()
    {
        isMoving = true;
        origin = transform.localPosition;
        timer = 0f;
    }

    private void Move(float value)
    {
        if (rectTransform)
            rectTransform.anchoredPosition3D = Vector3.Lerp(origin, points[index], curve.Evaluate(value));
        else
            transform.localPosition = Vector3.Lerp(origin, points[index], curve.Evaluate(value));
    }
}
