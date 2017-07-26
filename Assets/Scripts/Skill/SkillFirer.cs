using UnityEngine;
using System.Collections;

public class SkillFirer : MonoBehaviour {

    private static SkillFirer _instance;
    public static SkillFirer Instance
    {
        get
        {
            if (null == _instance)
            {
                GameObject gobj = new GameObject("SkillFirer");
                _instance = gobj.AddComponent<SkillFirer>();
            }
            return _instance;
        }
    }

	public bool CheckCanFireSkill(int slotId)
    {
        SkillCdData cdData = SkillDataMgr.Instance().GetSkillCdDataBySlotId(slotId);
        if (null == cdData)
            return false;
        if (cdData.IsInAction)
        {
            Util.LogAsvo("Cannot fire skill.Is Action. slot:" + slotId);
            return false;
        }
        return true;
    }

    void Update()
    {
        SkillDataMgr.Instance().UpdateAllActionTime();
    }

}
