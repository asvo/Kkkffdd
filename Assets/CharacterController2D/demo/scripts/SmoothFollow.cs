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

		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
            if (transform.position.x > rightborder)
            {
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
            }
		}
		else
		{
            if (transform.position.x < leftborder)
            {
                
            }
            else
            {
                var leftOffset = cameraOffset;
                leftOffset.x *= -1;
                transform.position = Vector3.SmoothDamp(transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime);
            }
		}
	}
	
}
