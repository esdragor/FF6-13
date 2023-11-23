using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMapCollision : MonoBehaviour
{
    private PlayerEntity _playerEntity = null;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_playerEntity == null)
        {
            _playerEntity = other.gameObject.GetComponent<PlayerEntity>();
        }
        if (_playerEntity == null) return;
        _playerEntity.ResetWantedPosition();
        _playerEntity.ResetMat();
        var position = other.transform.position;
        Vector3 direction = position - transform.position;
        position += direction.normalized * 0.1f;
        other.transform.position = position;
    }
}