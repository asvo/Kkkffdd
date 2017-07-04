using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Button BtnNormalAttack;

    void Awake()
    {
        BtnNormalAttack.onClick.AddListener(OnClickNormalAttack);        
    }

    private void OnClickNormalAttack()
    {
        GameManager.Instance().MainPlayer.FireSkill(0);
    }
}
