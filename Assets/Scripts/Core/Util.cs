using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    /// <summary>
    /// 主角寻找目标距离的怪物
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="dist"></param>
    /// <returns></returns>
    public static BaseEntity FindNereastTargetMonsterByDist(BaseEntity entity, float dist)
    {
        MoveDir faceDir = entity.MoveCtrl.GetCurrentFaceDir();
        List<Monster> monsters = MonsterManager.Instance().ActiveMonsters;
        float curDelta = float.MaxValue;
        BaseEntity findedMonster = null;
        for (int i = 0; i < monsters.Count; ++i)
        {
            float deltaPos = monsters[i].transform.position.x - entity.transform.position.x;
            if (faceDir == MoveDir.Right && deltaPos < 0)
                continue;
            if (faceDir == MoveDir.Left && deltaPos > 0)
                continue;
            float disttace = Mathf.Abs(deltaPos);
            if (disttace > dist)
                continue;
            if (curDelta > disttace)
            {
                curDelta = disttace;
                findedMonster = monsters[i];
            }
        }
        return findedMonster;
    }
}
