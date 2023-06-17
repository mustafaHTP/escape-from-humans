using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float _levelTime;
    [SerializeField] float _levelLoadDelayAmount;

    TMP_Text _timerTMP;
    CollisionHandler _collisionHandler;
    float _currentTime;
    bool _isTimerStarted;

    void Start()
    {
        _collisionHandler = GameObject.Find("DataRocket").GetComponent<CollisionHandler>();
        _currentTime = _levelTime;
        _levelLoadDelayAmount = 2f;
        _timerTMP = GetComponent<TMP_Text>();
        _timerTMP.text = _currentTime.ToString();
        _isTimerStarted = true;
    }

    void Update()
    {
        if (_isTimerStarted)
        {
            _currentTime -= Time.deltaTime;
            _timerTMP.text = _currentTime.ToString("N1");
            if (_currentTime < 0)
            {
                _currentTime = 0;
                _isTimerStarted = false;
                _collisionHandler.Invoke("ReloadLevel", _levelLoadDelayAmount);
            }
        }
    }

    public void ResetTimer()
    {
        _isTimerStarted = true;
        _currentTime = _levelTime;
    }

    public void StopTimer()
    {
        _isTimerStarted = false;
        _timerTMP.enabled = false;
    }
}
