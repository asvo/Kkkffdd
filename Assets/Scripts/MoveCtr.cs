using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class MoveCtr : MonoBehaviour {

    #region 1 - 变量
    public new Rigidbody2D rigidbody2D;
    /// <summary>
    /// 飞船移动速度
    /// </summary>
    public Vector2 speed = new Vector2(50, 50);

    // 存储运动
    private Vector2 movement;

    #endregion

    // Update is called once per frame
    void Update()
    {
        #region 运动控制

        // 2 - 获取轴信息
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputY != 0)
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        // 3 - 保存运动轨迹
        movement = new Vector2(speed.x * inputX, speed.y * inputY);

        #endregion
    }

    void FixedUpdate()
    {
        // 4 - 让游戏物体移动
        rigidbody2D.velocity = movement;
    }
}


