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

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(RandomMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTracking)
        {
            if (!_isMoving)
            {
                StartCoroutine(RandomMove(_speed));
            }
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
    
    private IEnumerator RandomMove(float duration)
    {
        _isMoving = true;
        yield return new WaitForSeconds(3f);
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
