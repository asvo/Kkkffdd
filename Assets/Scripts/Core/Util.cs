using UnityEngine;
using System.Collections;

public static class Util {

    /// <summary>
    /// 检查对象渲染器是否在摄像机的可见范围内
    /// </summary>
    /// <param name="renderer">渲染对象</param>
    /// <param name="camera">摄像机</param>
    /// <returns></returns>
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static T TryAddComponent<T>(GameObject go)where T:Component
    {
        if (go.GetComponent<T>() != null)
        {
           return go.GetComponent<T>();
        }
        else
        {
           return go.AddComponent<T>();
        }
    }

    public static int PlayerLayer
    {
        get
        {
            return LayerMask.NameToLayer("Player");
        }
    }

    public static int MonsterLayer
    {
        get
        {
            return LayerMask.NameToLayer("Monster");
        }
    }
}
