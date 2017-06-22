using UnityEngine;
using System.Collections;
using Prime31;


public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;

    public float leftborder;
    public float rightborder;
    public float manposition;
	
	
	public void Init(Transform followTarget)
	{
        target = followTarget;
		transform = gameObject.transform;
		_playerController = target.GetComponent<CharacterController2D>();
	}

    void Awake()
    {
        if (target != null)
        {
            transform = gameObject.transform;
            _playerController = target.GetComponent<CharacterController2D>();
        }
    }
	
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


    void updateCameraPosition()
    {
        if (target == null)
            return;

        if (_playerController == null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
            return;
        }

        if (transform.position.x <= leftborder)
        {
            if (Player.curMoveDir == MoveDir.Left)
            {
                _playerController.velocity.x = 0;
                Vector3.SmoothDamp(transform.position, new Vector3(leftborder, transform.position.y, transform.position.z), ref _smoothDampVelocity, smoothDampTime);
                //transform.position = new Vector3(leftborder, transform.position.y, transform.position.z);
            }
            else
            {
                if (CameraManager.Instance().MainCamera.WorldToScreenPoint(target.position).x < Screen.width *0.5f)
                {
                    _playerController.velocity.x = 0;
                }
            }
        }
        if (transform.position.x >= rightborder)
        {
            if (Player.curMoveDir == MoveDir.Right)
            {
                _playerController.velocity.x = 0;
                Vector3.SmoothDamp(transform.position, new Vector3(rightborder, transform.position.y, transform.position.z), ref _smoothDampVelocity, smoothDampTime);
                //transform.position = new Vector3(rightborder, transform.position.y, transform.position.z);
            }
            else
            {
                if (CameraManager.Instance().MainCamera.WorldToScreenPoint(target.position).x > Screen.width * 0.5f)
                {
                    _playerController.velocity.x = 0;
                }
            }
        }

        if (_playerController.velocity.x == 0)
        {

        }
        else if (_playerController.velocity.x > 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
        }
        else
        {
            var leftOffset = cameraOffset;
            leftOffset.x *= -1;
            transform.position = Vector3.SmoothDamp(transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime);
        }
    }
	
}
