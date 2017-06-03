using UnityEngine;
using System.Collections;

public class CameraCtr : MonoBehaviour {

    public Transform character;   //摄像机要跟随的人物 

    public float smoothTime = 0.01f;  //摄像机平滑移动的时间

    private Vector3 cameraVelocity = Vector3.zero;

    private Camera mainCamera;

    public Renderer backgroundRender;

    public float leftBorder = -11;
    public float rightBorder = 11;

    void Awake()
    {

        mainCamera = CameraManager.Instance().MainCamera;

    }

    void Update()
    {
        if (character.position.x <= leftBorder)
        {
            mainCamera.transform.position = new Vector3(leftBorder, 0, character.position.z - 5);
        }
        if (character.position.x >= rightBorder)
        {
            mainCamera.transform.position = new Vector3(rightBorder, 0, character.position.z - 5);
        }
        if (character.position.x >= leftBorder && character.position.x <= rightBorder)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, new Vector3(character.position.x, 0, character.position.z - 5), ref cameraVelocity, smoothTime);
        }
    }  
 
}
