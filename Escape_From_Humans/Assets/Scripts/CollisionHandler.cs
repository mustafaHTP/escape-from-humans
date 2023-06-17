using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float _delayAmount;
    [SerializeField] AudioClip _crashClip;
    [SerializeField] AudioClip _successClip;
    [SerializeField] ParticleSystem _crashParticle;
    [SerializeField] ParticleSystem _successParticle;
    [SerializeField] ParticleSystem _rocketBoostParticle;
    [SerializeField] ParticleSystem _rocketLeftBoostParticle;
    [SerializeField] ParticleSystem _rocketRightBoostParticle;

    Timer _timer;
    AudioSource _audioSource;
    int _currentSceneIndex;
    Movement _movement;
    bool _isTransitioning;
    bool _isCollisionEnabled;

    private void Start()
    {
        _timer =
        GameObject.Find("Main Camera").GetComponentInChildren<Timer>();
        if (_timer == null)
        {
            Debug.Log("timer not found...");
        }
        _isCollisionEnabled = true;
        _audioSource = GetComponent<AudioSource>();
        _movement = GetComponent<Movement>();
        _delayAmount = 2f;
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        ProcessDebugKeys();
    }
    private void OnCollisionEnter(Collision other)
    {
        //If any of the side rockets in collision, dont load scenes
        //So game continues,
        //Instead, turn off them
        if (IsSideBoosterCollision(other)) return;

        if (!_isTransitioning)
        {
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Finish":
                    StartSuccessSequence();
                    break;
                default:
                    StartCrashSequence();
                    break;
            }
        }
    }

    private bool IsSideBoosterCollision(Collision other)
    {
        bool isSideBoosterCollision = false;
        for (int i = 0; i < other.contactCount; ++i)
        {
            string objectName = other.GetContact(i).thisCollider.gameObject.name;
            string hitObjectName = other.gameObject.tag;
            if (objectName.CompareTo("LeftRocketBooster") == 0
                && !hitObjectName.Equals("Friendly") && !hitObjectName.Equals("Finish"))
            {
                Debug.Log("Left booster collision...");
                isSideBoosterCollision = true;
                _movement.IsRocketLeftBoostDamaged = true;

            }
            else if (objectName.CompareTo("RightRocketBooster") == 0
                && !hitObjectName.Equals("Friendly") && !hitObjectName.Equals("Finish"))
            {
                Debug.Log("Right booster collision...");
                isSideBoosterCollision = true;
                _movement.IsRocketRightBoostDamaged = true;
                break;
            }
            //switch (objectName)
            //{
            //    case "LeftRocketBooster":
            //        break;
            //    case "RightRocketBooster":
            //        Debug.Log("Right booster collision...");
            //        isSideBoosterCollision = true;
            //        _movement.IsRocketRightBoostDamaged = true;
            //        break;
            //    default:
            //        break;
            //}
        }
        return isSideBoosterCollision;
    }


    private void ProcessDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCollision();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetLevel(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetLevel(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetLevel(2);
        }

    }

    private void SetLevel(int sceneIndex)
    {
        _currentSceneIndex = sceneIndex;
        SceneManager.LoadScene(_currentSceneIndex);
    }

    private void ToggleCollision()
    {
        if (!_isCollisionEnabled)
        {
            EnableCollision(this.transform);
        }
        else
        {
            DisableCollision(this.transform);
        }
    }

    private void EnableCollision(Transform parent)
    {
        _isCollisionEnabled = true;
        EnableCollisionRecursive(parent);
    }
    private void DisableCollision(Transform parent)
    {
        _isCollisionEnabled = false;
        DisableCollisionRecursive(parent);
    }

    private void EnableCollisionRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            EnableCollisionRecursive(child);
        }
    }

    private void DisableCollisionRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            DisableCollisionRecursive(child);
        }
    }


    private void StopBoostParticlesOnLevelTransition()
    {
        _rocketRightBoostParticle.Stop();
        _rocketLeftBoostParticle.Stop();
        _rocketBoostParticle.Stop();
    }

    private void StartSuccessSequence()
    {
        _isTransitioning = true;
        _timer.StopTimer();
        Invoke("LoadNextLevel", _delayAmount);
        // Audio
        _audioSource.Stop();
        _audioSource.PlayOneShot(_successClip);
        // Particle
        StopBoostParticlesOnLevelTransition();
        _successParticle.Play();

        _movement.enabled = false;
    }

    private void StartCrashSequence()
    {
        _isTransitioning = true;
        _timer.StopTimer();
        Invoke("ReloadLevel", _delayAmount);
        // Audio
        _audioSource.Stop();
        _audioSource.PlayOneShot(_crashClip);
        // Particle
        StopBoostParticlesOnLevelTransition();
        _crashParticle.Play();

        _movement.enabled = false;
    }


    public void LoadNextLevel()
    {
        int nextSceneIndex = _currentSceneIndex + 1;
        if (nextSceneIndex != SceneManager.sceneCountInBuildSettings)
        {
            ++_currentSceneIndex;
        }
        SceneManager.LoadScene(_currentSceneIndex);
        _timer.ResetTimer();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(_currentSceneIndex);
        _timer.ResetTimer();
    }
}
