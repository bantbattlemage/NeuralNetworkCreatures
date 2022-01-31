using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float MaxDistanceToTarget;
    [SerializeField] private float MinimumDistanceToTarget;

    private Camera _camera;
    private float _currentDistance;
    private Vector3 _previousPosition;
    private float _scrollFactor = 20;

	private void Awake()
	{
        _currentDistance = MaxDistanceToTarget;
        _camera = GetComponent<Camera>();
        _camera.transform.position = Target.position;
        _camera.transform.Translate(new Vector3(0, 0, -_currentDistance));
    }

	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.mouseScrollDelta.y > 0 && _currentDistance > MinimumDistanceToTarget)
        {
            _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);

            if(_currentDistance - (_scrollFactor) >= MinimumDistanceToTarget)
			{
                _currentDistance -= _scrollFactor;
			}
            else
			{
                _currentDistance--;
			}
        }
        else if (Input.mouseScrollDelta.y < 0 && _currentDistance < MaxDistanceToTarget)
        {
            _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);

            if (_currentDistance + (_scrollFactor) <= MaxDistanceToTarget)
            {
                _currentDistance += _scrollFactor;
            }
            else
            {
                _currentDistance++;
            }
        }

        if (Input.GetMouseButton(0) || Input.mouseScrollDelta.y != 0)
        {
            Vector3 newPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = _previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            _camera.transform.position = Target.position;

            if(Input.GetMouseButton(0))
			{
                _camera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                _camera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
            }

            _camera.transform.Translate(new Vector3(0, 0, -_currentDistance));

            _previousPosition = newPosition;
        }
    }
}
