using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

// from Fusion tutorial: https://doc.photonengine.com/fusion/current/tutorials/shared-mode-basics/5-remote-procedure-calls
public class PlayerShooter : NetworkBehaviour
{
    [SerializeField] InputAction attack;
    [SerializeField] InputAction attackLocation;
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] float prefabHeight = 1.5f;
    [SerializeField] float offset = 0.5f;
    private float isRightDirection = 90;
    private float spawnDelay = 0.2f;
    private float currTimer = 0.2f;

    private void OnEnable() { attack.Enable(); attackLocation.Enable(); }
    private void OnDisable() { attack.Disable(); attackLocation.Disable(); }
    void OnValidate()
    {
        // Provide default bindings for the input actions. Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (attack == null)
            attack = new InputAction(type: InputActionType.Button);
        if (attack.bindings.Count == 0)
            attack.AddBinding("<Mouse>/leftButton");

        if (attackLocation == null)
            attackLocation = new InputAction(type: InputActionType.Value, expectedControlType: "Vector2");
        if (attackLocation.bindings.Count == 0)
            attackLocation.AddBinding("<Mouse>/position");
    }

    void Update()
    {
        if (!HasStateAuthority) return;
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
                if (gameObject.transform.rotation.eulerAngles.y == isRightDirection)
                {
                    spawnPos.x += offset;
                }
                else
                {
                    spawnPos.x -= offset;

                }
                Quaternion rocketRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.y);
                Runner.Spawn(prefabToSpawn, spawnPos, rocketRotation);
                currTimer = spawnDelay;
            }
        }

    }




}