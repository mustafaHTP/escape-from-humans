using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    [HideInInspector] public bool IsRocketRightBoostDamaged;
    [HideInInspector] public bool IsRocketLeftBoostDamaged;

    [Header("Audio")]
    [SerializeField] AudioClip _rocketBoostClip;

    [Header("Speed And Boost")]
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _boostAmount;

    [Header("Particles")]
    [SerializeField] ParticleSystem _rocketBoostParticle;
    [SerializeField] ParticleSystem _rocketLeftBoostParticle;
    [SerializeField] ParticleSystem _rocketRightBoostParticle;

    [Header("Input Actions")]
    [SerializeField] InputAction _boostInput;
    [SerializeField] InputAction _rotateInput;

    Rigidbody _rigidbody;
    AudioSource _audioSource;
    private void OnEnable()
    {
        _boostInput.Enable();
        _rotateInput.Enable();
    }

    private void OnDisable()
    {
        _boostInput.Disable();
        _rotateInput.Disable();
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _boostAmount = 1000f;
        _rotationSpeed = 100f;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        ProcessBoost();
        ProcessRotation();
    }

    void ProcessBoost()
    {
        if (_boostInput.IsPressed())
        {
            StartBoost();

        }
        else
        {
            StopBoost();
        }
    }

    private void StopBoost()
    {
        _rocketBoostParticle.Stop();
        _audioSource.Stop();
    }

    private void StartBoost()
    {
        if (!_audioSource.isPlaying)
        {
            _rocketBoostParticle.Play();
            _audioSource.PlayOneShot(_rocketBoostClip);
        }
        _rigidbody.AddRelativeForce(Vector3.up * _boostAmount * Time.deltaTime);
    }

    void ProcessRotation()
    {
        float rotateInput = _rotateInput.ReadValue<float>();
        int rotateDirection = rotateInput.CompareTo(0f);
        Debug.Log("input: " + rotateInput + " direction: " + rotateDirection);
        if (rotateDirection < 0 && !IsRocketLeftBoostDamaged)
        {
            RotateLeft();
        }
        else if (rotateDirection > 0 && !IsRocketRightBoostDamaged)
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }
    private void RotateLeft()
    {
        ApplyRotation(1);
        if (!_rocketLeftBoostParticle.isPlaying)
            _rocketLeftBoostParticle.Play();
    }

    private void RotateRight()
    {
        ApplyRotation(-1);
        if (!_rocketRightBoostParticle.isPlaying)
            _rocketRightBoostParticle.Play();
    }
    private void StopRotating()
    {
        _rocketLeftBoostParticle.Stop();
        _rocketRightBoostParticle.Stop();
    }


    private void ApplyRotation(float rotationDirection)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationDirection * Time.deltaTime * _rotationSpeed);
        _rigidbody.freezeRotation = false;
    }

}
