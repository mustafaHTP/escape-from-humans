using UnityEngine;

public class PoliceSounder : MonoBehaviour
{
    [SerializeField] float _maxDistanceToEmitSound;
    [SerializeField] GameObject _rocket;

    AudioSource _audioSource;
    float _initialVolume;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _initialVolume = _audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(_rocket.transform.position, transform.position);
        float volume = 1f - Mathf.Clamp01(distance / _maxDistanceToEmitSound);
        _audioSource.volume = volume * _initialVolume;
        Debug.Log("Police Volum: " + _audioSource.volume);
        Debug.Log("Distance: " + distance);
        Debug.Log("Volum: " + volume);
        if (!_audioSource.isPlaying)
            _audioSource.Play();

    }
}
