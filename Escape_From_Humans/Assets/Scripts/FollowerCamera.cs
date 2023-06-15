using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    [SerializeField] float _xCameraOffset;
    [SerializeField] float _yCameraOffset;
    [SerializeField] float _zCameraOffset;
    [SerializeField] GameObject _dataRocket;
    Vector3 _offset;
    void Start()
    {
        _offset = new Vector3(_xCameraOffset, _yCameraOffset, _zCameraOffset);
        transform.position = _dataRocket.transform.position + _offset;
    }
    void Update()
    {
        transform.position = _dataRocket.transform.position + _offset;
    }
}
