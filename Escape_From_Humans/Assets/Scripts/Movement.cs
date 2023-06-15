using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool IsRocketRightBoostDamaged;
    public bool IsRocketLeftBoostDamaged;

    [SerializeField] float _rotationSpeed;
    [SerializeField] float _boostAmount;
    [SerializeField] AudioClip _rocketBoostClip;
    [SerializeField] ParticleSystem _rocketBoostParticle;
    [SerializeField] ParticleSystem _rocketLeftBoostParticle;
    [SerializeField] ParticleSystem _rocketRightBoostParticle;

    Rigidbody _rigidbody;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //IsRocketRightBoostDamaged = false;
        //IsRocketLeftBoostDamaged = false;
        _audioSource = GetComponent<AudioSource>();
        _boostAmount = 1000f;
        _rotationSpeed = 100f;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
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
        if (Input.GetKey(KeyCode.Space))
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
        if (Input.GetKey(KeyCode.LeftArrow) && !IsRocketLeftBoostDamaged)
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !IsRocketRightBoostDamaged)
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
