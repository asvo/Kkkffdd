using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Button BtnNormalAttack;
    public Button BtnSkill01;
    public Button BtnSkill02;

    void Awake()
    {
        BtnNormalAttack.onClick.AddListener(OnClickNormalAttack);
        BtnSkill01.onClick.AddListener(OnClickFireSkill01);
        BtnSkill02.onClick.AddListener(OnClickFireSkill02);
    }

    private void OnClickNormalAttack()
    {
        GameManager.Instance().MainPlayer.FireSkill(0);
    }

    private void OnClickFireSkill01()
    {
        GameManager.Instance().MainPlayer.FireSkill(1);
    }

    private void OnClickFireSkill02()
    {
        GameManager.Instance().MainPlayer.FireSkill(2);
    }
}
