using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class CameraManager : Single<CameraManager> {

    private Camera mainCamera = null;
    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            return mainCamera;
        }
        private set
        {
            mainCamera = value;
        }
    }

    public void LookAt(Transform target)
    {
        MainCamera.transform.LookAt(target);
        TweenRotYFromAToB(298,361.7f,1);
    }

    private void TweenRotYFromAToB(float A,float B,float duration = 0.5f)
    {
        DOTween.To(() => MainCamera.transform.eulerAngles, x => MainCamera.transform.eulerAngles = x, new Vector3(0, A, 0), duration).OnComplete(delegate { TweenRotYFromAToB(B, -A, duration); });
    }
}

