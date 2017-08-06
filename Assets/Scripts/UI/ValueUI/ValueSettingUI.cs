﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJson;
using ValueModule;
using System.Reflection;
using System;
public class ValueSettingUI : MonoBehaviour {

    //setting btns
    public Button[] ListBtns;

    public GameObject ExtraGobj;

    public Transform Grid;
    public GameObject InputPrefab;
    public Text TxtCurrentInputMode;
    private Dictionary<string, InputField> settingInputs = new Dictionary<string, InputField>();

    public SkillSettingItem mSkillSettingItem;

    public enum SettingPanelType
    {
        Monster = 0,
        Player,
        Skill,
        Max
    }
    private SettingPanelType mSettingPaneType;
    
    public bool bPlayerSetting = false;

    public void OnClickShowMosnter()
    {
        mSettingPaneType = SettingPanelType.Monster;        
        ClickShowSetting();
    }

    public void OnClickShowPlayer()
    {
        mSettingPaneType = SettingPanelType.Player;
        ClickShowSetting();
    }

    public void OnClickShowPlayerSkill()
    {
        mSettingPaneType = SettingPanelType.Skill;
        ClickShowSetting();
    }

    private void ClickShowSetting()
    {
        //disable all btns;
        for(int i = 0; i < ListBtns.Length; ++i)
        {
            ListBtns[i].interactable = false;
        }
        if (mSettingPaneType == SettingPanelType.Skill)
        {
            ExtraGobj.SetActive(true);
            Grid.gameObject.SetActive(false);
            mSkillSettingItem.gameObject.SetActive(false);
        }
        else
        {
            ExtraGobj.SetActive(false);
            //之前的逻辑
            ShowSetting(mSettingPaneType == SettingPanelType.Player);
        }
        ShowCurrentInputMode();
    }

    private void ShowSetting(bool isPlayer)
    {
        bPlayerSetting = isPlayer;
        ValueManager.Instance().Load(bPlayerSetting);        
        if (isPlayer)
        {
            InitSettingInput(ValueManager.Instance().PlayerValueSettings);
        }
        else
        {
            InitSettingInput(ValueManager.Instance().MonsterValueSettings);
        }
        Grid.gameObject.SetActive(true);        
    }

    private void Save()
    {
        if (mSettingPaneType == SettingPanelType.Skill)
        {
            mSkillSettingItem.SaveToCorrespondSkill();
        }
        else
        {
            SaveJsonKeyEnumPair();
            settingInputs.Clear();
        }
    }

    //private void SaveJsonKeyEnumPair()
    //{
    //    if (settingInputs == null || settingInputs.Count == 0)
    //    {
    //        return;
    //    }
    //    JsDataBaseValue jsData = null;
    //    if (bPlayerSetting)
    //    {
    //        //jsData = ValueManager.Instance().PlayerValueSettings;
    //    }
    //    else
    //    {
    //        //jsData = ValueManager.Instance().MonsterValueSettings;
    //    }

    //    foreach (var pair in settingInputs)
    //    {
    //        float value = 0;
    //        if (float.TryParse(pair.Value.text, out value))
    //        {
    //            if (jsData.dic_BaseValues.ContainsKey(pair.Key.ToString()))
    //            {
    //                jsData.dic_BaseValues[pair.Key.ToString()] = value;
    //            }
    //            else
    //            {
    //                jsData.dic_BaseValues.Add(pair.Key.ToString(), value);
    //            }
    //        }
    //    }

    //    ValueManager.Instance().Save(bPlayerSetting);        
    //}

    //private void InitSettingInput(string[] keys)
    //{
    //    Util.SetChildsActive(Grid, false);
    //    settingInputs.Clear();
    //    JsDataBaseValue jsData = null;
    //    if (bPlayerSetting)
    //    {
    //        //jsData = ValueManager.Instance().PlayerValueSettings;
    //    }
    //    else
    //    {
    //        //jsData = ValueManager.Instance().MonsterValueSettings;
    //    }

    //    for (int i = 0; i < keys.Length; i++)
    //    {
    //        GameObject obj = CreateInput(Grid, InputPrefab, i);
    //        InputField _input = obj.GetComponent<InputField>();
    //        e_BaseValue key = (e_BaseValue)System.Enum.Parse(typeof(e_BaseValue), keys[i]);
    //        _input.transform.FindChild("Desc").GetComponent<Text>().text = keys[i];
    //        if (jsData.dic_BaseValues.ContainsKey(keys[i]))
    //        {
    //            _input.text = jsData.dic_BaseValues[keys[i]].ToString();
    //            _input.placeholder.GetComponent<Text>().text = jsData.dic_BaseValues[keys[i]].ToString(); 
    //        }
    //        else
    //        {
    //            _input.placeholder.GetComponent<Text>().text = keys[i];
    //        }
    //        settingInputs.Add(key, _input);
    //        obj.SetActive(true);
    //    }
    //}

    //private GameObject CreateInput(Transform tParent, GameObject prefab,int index = 0)
    //{
    //    GameObject newChild = null;
    //    if (tParent.childCount > index)
    //    {
    //        newChild = tParent.GetChild(index).gameObject;
    //    }
    //    else
    //    {
    //        newChild = Util.AddChildToTarget(prefab, tParent.gameObject);
    //    }

    //    return newChild;
    //}
    #region 基础数值设置
    private void SaveJsonKeyEnumPair()
    {
        ICharacterValue data = null;
        if (bPlayerSetting)
        {
            data = ValueManager.Instance().PlayerValueSettings;
        }
        else
        {
            data = ValueManager.Instance().MonsterValueSettings;
        }

        Type type = data.GetType();
        FieldInfo fields = null;

        foreach (var pair in settingInputs)
        {
            float value = 0;
            if (float.TryParse(pair.Value.text, out value))
            {
                fields = type.GetField(pair.Key);
                if (fields.FieldType == typeof(int))
                {
                    fields.SetValue(data, (int)value);
                }
                else if (fields.FieldType == typeof(float))
                {
                    fields.SetValue(data, value);
                }
            }
        }

        ValueManager.Instance().Save(bPlayerSetting);
    }

    private void InitSettingInput(IBaseValue valueData)
    {
        Type type = valueData.GetType();
        FieldInfo[] fields = type.GetFields();

        Util.SetChildsActive(Grid, false);

        for (int i = 0; i < fields.Length; i++)
        {
            object value = fields[i].GetValue(valueData);
            string key = fields[i].Name;

            GameObject obj = CreateInput(Grid, InputPrefab, i);
            InputField _input = obj.GetComponentInChildren<InputField>();

            _input.transform.FindChild("Desc").GetComponent<Text>().text = key;
            _input.text = value.ToString();
            _input.placeholder.GetComponent<Text>().text = value.ToString();

            settingInputs.Add(key, _input);
            obj.SetActive(true);
        }
    }

    private GameObject CreateInput(Transform tParent, GameObject prefab, int index = 0)
    {
        if (tParent.childCount > index)
        {
            return tParent.GetChild(index).gameObject;
        }
        else
        {
            return Util.AddChildToTarget(prefab, tParent.gameObject);
        }
    }
    #endregion

    /// <summary>
    /// 切换输入方式
    /// </summary>
    public void OnClickChangeInputMode()
    {
        GameManager.Instance().ChangeInputMode();
        ShowCurrentInputMode();
    }

    private void ShowCurrentInputMode()
    {
        string str = "当前：";
        if (GameManager.CurrentInputMode == MoveInput.InputModeStyle.JoyStick)
            str += "摇杆控制";
        else
            str += "按钮控制";
        TxtCurrentInputMode.text = str;
    }

    #region Skill-Setting-Btns

    public void ShowSkillPaneBySlot(int slotId)
    {
        mSkillSettingItem.gameObject.SetActive(true);
        mSkillSettingItem.ShowSetting(slotId);
    }

    #endregion

    public void OnClickCancel()
    {
        ShowInitBtnAndPane();
    }

    public void OnClickSave()
    {
        ShowInitBtnAndPane();
        Save();
    }

    private void ShowInitBtnAndPane()
    {
        for (int i = 0; i < ListBtns.Length; ++i)
        {
            ListBtns[i].interactable = true;
        }
        ExtraGobj.SetActive(false);
        Grid.gameObject.SetActive(false);
    }
}
