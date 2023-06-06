using Fusion;
using UnityEngine;
using System.Collections;

public class Rocket : NetworkBehaviour
{
    private float speed = 20f;
    private ConstantForce constForce;
    private float rotationAngle;
    private float gravity = 10f;
    private float isRightDirection = 90;

    private float delay = 5f;

    public override void Spawned()
    {
        constForce = GetComponent<ConstantForce>();
        rotationAngle = transform.rotation.eulerAngles.z;
    }

    public override void FixedUpdateNetwork()
    {
        if (constForce != null)
        {
            constForce.force = new Vector3(rotationAngle == isRightDirection ? -speed : speed, gravity, 0);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (gameObject && other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

}
