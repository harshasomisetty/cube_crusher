using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed = .5f;

    void Update()
    {
        transform.Rotate(Vector3.up * 360 * rotationSpeed * Time.deltaTime);

    }
}
