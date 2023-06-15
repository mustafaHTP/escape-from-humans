using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 _movementVector;
    [SerializeField] float _period;

    Vector3 _startingPoint;

    void Start()
    {
        _startingPoint = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Check divide by zero error
        if (_period <= Mathf.Epsilon) return;
        // continually growing over time
        float cycles = Time.time / _period;
        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        // _movementFactor between 0 and 1
        float _movementFactor = (rawSinWave + 1f) / 2;

        transform.localPosition = _startingPoint + (_movementVector * _movementFactor);
    }
}
