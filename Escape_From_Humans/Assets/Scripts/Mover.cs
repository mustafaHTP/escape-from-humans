using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] Vector3 _movementVector;
    [SerializeField] float _movementSpeed;

    float _distance;
    Vector3 _startingPoint;
    Vector3 _targetPoint;
    void Start()
    {
        _startingPoint = transform.position;
        _targetPoint =  _startingPoint + _movementVector;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _movementSpeed * Time.deltaTime);
    }
}
