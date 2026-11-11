using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckIndicator : MonoBehaviour
{
    [SerializeField] private Transform checkersParent;
    [SerializeField] private Gradient colorPalette;
    [SerializeField] private float rangeTravelInSeconds = 5f;
    [Header("Debug")] [SerializeField] private SkillCheckReceiver skillCheckReceiver;
    private List<Image> _checkers = new();
    private float _rangeTimer;
    private float _rangeProgress;
    private bool _setTimer = false;
    
    private Coroutine TriggerCheckCoroutine;
    public Action<float> TriggerCheckResult;

    private void Start()
    {
        foreach (Transform child in checkersParent)
            _checkers.Add(child.GetComponent<Image>());
        SetColors();
        
        _rangeTimer = 0f;
        _setTimer = true;
        skillCheckReceiver.ActionPressed += TriggerCheck;
    }
    private void SetColors()
    {
        for (int i = 0; i < _checkers.Count; i++)
        {
            Color color = colorPalette.Evaluate(i / (float)_checkers.Count);
            color.a = 0.2f;
            _checkers[i].color = color;
        }
    }

    private void LateUpdate()
    {
        if (!_setTimer) return;
        _rangeTimer += Time.deltaTime;
        if (_rangeTimer >= rangeTravelInSeconds)
            Reset();
        
        _rangeProgress = _rangeTimer / rangeTravelInSeconds;
        TrackProgress(_rangeProgress);
    }

    private void TrackProgress(float progress)
    {
        for (int i = 0; i < _checkers.Count; i++)
        {
            Color color = _checkers[i].color;
            color.a = 0.2f;
            _checkers[i].color = color;

            if (i / (float)_checkers.Count >= progress)
                break;
            
            color.a = 1f;
            _checkers[i].color = color;
        }
    }

    private void Reset()
    {
        foreach (Image checker in _checkers)
        {
            Color color = checker.color;
            color.a = 0.2f;
            checker.color = color;
        }
        _rangeTimer = 0f;
        _setTimer = true;
    }

    private void TriggerCheck()
    {
        // Debug.Log("Stopped range at:  " + _rangeProgress);
        if(TriggerCheckCoroutine != null)
            StopCoroutine(TriggerCheckCoroutine);
        TriggerCheckCoroutine = StartCoroutine(TriggerCheck_Coroutine(_rangeProgress));
    }

    private IEnumerator TriggerCheck_Coroutine(float progress)
    {
        foreach (Image checker in _checkers)
        {
            Color color = colorPalette.Evaluate(progress);
            color.a = 1f;
            checker.color = color;
        }
        
        TriggerCheckResult?.Invoke(progress);
        _setTimer = false;
        yield return new WaitForSeconds(1f);
        SetColors();
        Reset();
    }
}