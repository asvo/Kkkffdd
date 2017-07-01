using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class Util
{

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

    public static T TryAddComponent<T>(GameObject go) where T : Component
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

    public static int GroundLayer
    {
        get
        {
            return LayerMask.NameToLayer("Ground");
        }
    }

    public static int WallLayer
    {
        get
        {
            return LayerMask.NameToLayer("Wall");
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

    public static GameObject AddChildToTarget(GameObject child, GameObject target)
    {
        GameObject obj = GameObject.Instantiate(child);
        obj.transform.SetParent(target.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    public static void SetChildsActive(Transform target, bool isActive)
    {
        foreach (Transform child in target)
        {
            if (child != null)
            {
                child.gameObject.SetActive(isActive);
            }
        }
    }

    public static void SaveJson(object parm, string path)
    {
        string jsonTxt = JsonFx.Json.JsonWriter.Serialize(parm);
        //string jsonTxt = JsonUtility.ToJson(parm);
        string savePath = Application.persistentDataPath + "/" + path + ".json";
        //Debug.LogError(savePath + "," + jsonTxt);
        System.IO.File.WriteAllText(savePath, jsonTxt);
    }

    public static T LoadJson<T>(string path)
    {
        string savePath = Application.persistentDataPath + "/" + path + ".json";
        string jsonTxt = File.ReadAllText(savePath);
        //Debug.LogError(savePath);
        T t = JsonFx.Json.JsonReader.Deserialize<T>(jsonTxt);
        //T t = JsonUtility.FromJson<T>(jsonTxt);
        return t;
    }

    public static bool CheckHaveJsonFile(string path)
    {
        string savePath = Application.persistentDataPath + "/" + path + ".json";
        bool bExists = System.IO.File.Exists(savePath);
        return bExists;
    }

    public static void LogAsvo(string content)
    {
        if (!GameManager.Instance().LogAsvo)
            return;
        Debug.Log(content);
    }
}
