using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    private const float gravity = 9.81f;
    
    Vector3 gravityVelocity;

    [SerializeField] private float gravityScale = 3f;

    private void FixedUpdate()
    {
        gravityVelocity.y += gravity * gravityScale * Time.deltaTime;
        transform.Translate(gravityVelocity);
    }

    private void GravityHandler()
    {
        gravityVelocity.y += gravity * gravityScale * Time.deltaTime;
        //characterController.Move(gravityVelocity);
    }
}
