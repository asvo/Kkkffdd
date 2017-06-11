using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour {

	//这里暂用单例吧。。结算ui只有一个
    private static GameOverUI mInstance = null;

    public static GameOverUI Instance
    {
        get
        {
            if (null == mInstance)
            {
                Object obj = Resources.Load("UIPrefab/GameOverUI");
                GameObject gobj = GameObject.Instantiate(obj) as GameObject;
                gobj.transform.SetParent(GameManager.Instance().UiRoot);
                gobj.transform.localPosition = Vector3.zero;
                gobj.transform.localScale = Vector3.one;
                mInstance = Util.TryAddComponent<GameOverUI>(gobj);
            }
            return mInstance;
        }
    }
    
    void Awake()
    {
        mInstance = this;
        Button btnRestart = transform.FindChild("BtnRestart").GetComponent<Button>();
        btnRestart.onClick.AddListener(OnClickRestart);        
    }

    public void HideUi()
    {
        gameObject.SetActive(false);
    }

    public void ShowUi()
    {
        gameObject.SetActive(true);
    }

    private void OnClickRestart()
    {
        GameManager.Instance().StartGame();
    }
}
