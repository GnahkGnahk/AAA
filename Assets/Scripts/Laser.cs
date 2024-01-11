using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            EnableLaser();
        }
        if (Input.GetButton("Fire1"))
        {
            UpdateLaser();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            DisableLaser();
        }
    }

    private void DisableLaser()
    {
        throw new NotImplementedException();
    }

    private void UpdateLaser()
    {
        throw new NotImplementedException();
    }

    private void EnableLaser()
    {
        throw new NotImplementedException();
    }
}
