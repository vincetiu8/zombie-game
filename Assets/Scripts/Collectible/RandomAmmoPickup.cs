using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAmmoPickup : AmmoPickup
{
    [SerializeField] [Range(1, 50)] private int _minAmt;
    [SerializeField] [Range(1, 50)] private int _maxAmt;

    private void Awake()
    {
        _dropAmount = Random.Range(_minAmt, _maxAmt);
    }
}