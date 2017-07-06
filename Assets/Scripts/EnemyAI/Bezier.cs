using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 二次方贝塞尔曲线：B(t) = (1 - t)^2 * P_0 + 2t(1 - t)P_1 + t^2 * P_2 ,{t in [0,1]}。
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Bezier
{
    private float t = 0;

    //start point
    float startPointX = 0;
    float startPointY = 0;
    //center point
    float centerPointX = 0;
    float centerPointY = 0;
    //end point
    float endPointX = 0;
    float endPointY = 0;

    public float _CurveX = 0;
    public float _CurveY = 0;

    public float duration;
    public float height = 0;

    private Transform m_Transform;

    public Bezier(Transform transform, float height, Vector2 StartPoint, Vector2 EndPoint, float duration = 1)
    {
        this.m_Transform = transform;
        this.height = height;
        this.duration = duration;

        startPointX = StartPoint.x;
        startPointY = StartPoint.y;

        endPointX = EndPoint.x;
        endPointY = EndPoint.y;

        centerPointX = (startPointX + endPointX) / 2;
        centerPointY = (startPointY + endPointY) / 2 + height;
    }

    public Vector2 Update()
    {
        t = t + Time.deltaTime / duration;
        if (t >= 1)
        {
            t = 0;
        }       

        _CurveX = (((1 - t) * (1 - t)) * startPointX) + (2 * t * (1 - t) * centerPointX) + ((t * t) * endPointX);
        _CurveY = (((1 - t) * (1 - t)) * startPointY) + (2 * t * (1 - t) * centerPointY) + ((t * t) * endPointY);

        m_Transform.position = new Vector2(_CurveX, _CurveY);
        return new Vector2(_CurveX, _CurveY);
    }
}

