using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJson;
public class ValueSettingUI : MonoBehaviour {

    public Transform Grid;
    public GameObject InputPrefab;
    private Dictionary<e_BaseValue, InputField> settingInputs = new Dictionary<e_BaseValue, InputField>();

    public bool bPlayerSetting = false;

    public void ShowSetting(bool isPlayer)
    {
        bPlayerSetting = isPlayer;
        ValueManager.Instance().Load(bPlayerSetting);
        InitSettingInput(System.Enum.GetNames(typeof(e_BaseValue)));
        Grid.gameObject.SetActive(true);
    }

    public void Save()
    {
        JsDataBaseValue jsData = null;
        if (bPlayerSetting)
        {
            jsData = ValueManager.Instance().PlayerValueSettings;
        }
        else
        {
            jsData = ValueManager.Instance().MonsterValueSettings;
        }

        foreach (var pair in settingInputs)
        {
            float value = 0;
            if (float.TryParse(pair.Value.text, out value))
            {
                if (jsData.dic_BaseValues.ContainsKey(pair.Key.ToString()))
                {
                    jsData.dic_BaseValues[pair.Key.ToString()] = value;
                }
                else
                {
                    jsData.dic_BaseValues.Add(pair.Key.ToString(), value);
                }
            }
        }

        ValueManager.Instance().Save(bPlayerSetting);
        Grid.gameObject.SetActive(false);
    }

    private void InitSettingInput(string[] keys)
    {
        Util.SetChildsActive(Grid, false);
        settingInputs.Clear();
        JsDataBaseValue jsData = null;
        if (bPlayerSetting)
        {
            jsData = ValueManager.Instance().PlayerValueSettings;
        }
        else
        {
            jsData = ValueManager.Instance().MonsterValueSettings;
        }

        for (int i = 0; i < keys.Length; i++)
        {
            GameObject obj = CreateInput(Grid, InputPrefab, i);
            InputField _input = obj.GetComponent<InputField>();
            e_BaseValue key = (e_BaseValue)System.Enum.Parse(typeof(e_BaseValue), keys[i]);
            if (jsData.dic_BaseValues.ContainsKey(keys[i]))
            {
                _input.placeholder.GetComponent<Text>().text = jsData.dic_BaseValues[keys[i]].ToString(); 
            }
            else
            {
                _input.placeholder.GetComponent<Text>().text = keys[i];
            }
            settingInputs.Add(key, _input);
            obj.SetActive(true);
        }
    }

    private GameObject CreateInput(Transform tParent, GameObject prefab,int index = 0)
    {
        GameObject newChild = null;
        if (tParent.childCount > index)
        {
            newChild = tParent.GetChild(index).gameObject;
        }
        else
        {
            newChild = Util.AddChildToTarget(prefab, tParent.gameObject);
        }

        return newChild;
    }
}

