using Fusion;
using UnityEngine;
using System.Collections;

public class Rocket : NetworkBehaviour
{
    private float speed = 20f;
    private ConstantForce constForce; // This is a Unity component
    private float rotationAngle;
    private float gravity = 10f;
    private float isRightDirection = 90;

    private float delay = 5f;

    public override void Spawned()
    {
        constForce = GetComponent<ConstantForce>();
        // Get the rotation of the spawned rocket
        rotationAngle = transform.rotation.eulerAngles.z;
    }

    public override void FixedUpdateNetwork()
    {
        if (constForce != null)
        {
            // Move on the axis away from the player that shot it
            float speedDirection = rotationAngle == isRightDirection ? -speed : speed;
            constForce.force = new Vector3(speedDirection, gravity, 0);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (gameObject && other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            // Destroy the player that was hit! 
            Destroy(other.gameObject);
        }
    }

}
