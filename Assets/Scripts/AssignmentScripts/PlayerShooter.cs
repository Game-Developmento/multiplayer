using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

// from Fusion tutorial: https://doc.photonengine.com/fusion/current/tutorials/shared-mode-basics/5-remote-procedure-calls
public class PlayerShooter : NetworkBehaviour
{
    [SerializeField] InputAction attack;
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] float prefabHeight = 1.5f;
    [SerializeField] float offset = 0.5f;
    private float isRightDirection = 90;
    private float spawnDelay = 0.2f;
    private float currTimer = 0.2f;

    private void OnEnable() { attack.Enable(); }
    private void OnDisable() { attack.Disable(); }
    void OnValidate()
    {
        // Provide default bindings for the input actions
        if (attack == null)
            attack = new InputAction(type: InputActionType.Button);
        if (attack.bindings.Count == 0)
            attack.AddBinding("<Mouse>/leftButton");
    }

    void Update()
    {
        if (!HasStateAuthority) return;
        // Delay to avoid spam shooting
        if (currTimer > 0)
        {
            currTimer -= Time.deltaTime;
        }
        else
        {  
            if (attack.WasPerformedThisFrame())
            {
                Vector3 spawnPos = transform.position;
                spawnPos.y += prefabHeight;
                // Add offset so the rocket spawns in front of the player
                if (gameObject.transform.rotation.eulerAngles.y == isRightDirection)
                {
                    spawnPos.x += offset;
                }
                else
                {
                    spawnPos.x -= offset;

                }
                // Rotate rocket to the direction it is shot
                Quaternion rocketRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.y);
                Runner.Spawn(prefabToSpawn, spawnPos, rocketRotation);
                currTimer = spawnDelay;
            }
        }

    }




}