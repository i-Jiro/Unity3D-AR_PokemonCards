using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PokemonEntity : MonoBehaviour
{
    [SerializeField] private float _xMaxMoveDistance;
    [SerializeField] private float _zMaxMoveDistance;
    [SerializeField] private float _speed = 1f;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private bool _isTracking = false;
    private bool _isMoving = false;
    private bool _isGettingPowerUp;

    private void OnEnable()
    {
        TrackablesManager.Instance.FoundPowerUp += GotoPowerUp;
    }

    private void OnDisable()
    {
        TrackablesManager.Instance.FoundPowerUp -= GotoPowerUp;
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isTracking) return;
        if (!_isMoving && !_isGettingPowerUp)
        {
            StartCoroutine(RandomMove(_speed));
        }
    }
    public void OnFoundTarget()
    {
        _isTracking = true;
    }

    public void OnLostTarget()
    {
        //reset to initial position.
        transform.localPosition = Vector3.zero;
        _isTracking = false;
    }

    private void GotoPowerUp(GameObject powerUpCard)
    {
        if (!_isTracking) return;
        _isGettingPowerUp = true;
        Debug.Log("Moving to power up.");
        StopAllCoroutines();
        _isMoving = false;
        var powerUp = powerUpCard.GetComponent<PowerUp>();
        Vector3 targetRelativePos = this.transform.InverseTransformPoint(powerUp.PowerUpOrb.transform.position);
        StartCoroutine(MoveToPowerUp(targetRelativePos, _speed));
    }

    private IEnumerator MoveToPowerUp(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.localPosition;
        _animator.SetBool("isMoving", true);
        float startTime = 0;
        
        if (targetPos.x > startPos.x)
        {
            _renderer.flipX = true;
        }
        else
        {
            _renderer.flipX = false;
        }
        
        while (startTime <= duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, startTime/duration);
            startTime += Time.deltaTime;
            yield return null;
        }
        
        _animator.SetBool("isMoving", false);
        _animator.SetTrigger("PowerUp");
        yield return new WaitForSeconds(3f);
        _isGettingPowerUp = false;
    }
    
    private IEnumerator RandomMove(float duration)
    {
        _isMoving = true;
        yield return new WaitForSeconds(3f); // Wait 3 seconds before moving again.
        _animator.SetBool("isMoving", true);
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos;
        float startTime = 0;
        targetPos.x = startPos.x + Random.Range(-_xMaxMoveDistance, _xMaxMoveDistance);
        targetPos.y = startPos.y;
        targetPos.z = startPos.z + Random.Range(-_zMaxMoveDistance, _zMaxMoveDistance);
        
        if (targetPos.x > startPos.x)
        {
            _renderer.flipX = true;
        }
        else
        {
            _renderer.flipX = false;
        }
        
        while (startTime <= duration)
        {
            transform.localPosition = Vector3.Lerp(startPos,targetPos, startTime/duration);
            startTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;
        _isMoving = false;
        _animator.SetBool("isMoving", false);
    }
}
