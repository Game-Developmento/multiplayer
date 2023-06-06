using Fusion;
using UnityEngine;
using System.Linq;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    private float rightDirectionAngle = 90;
    private float leftDirectionAngle = 270;
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            int playerCount = Runner.ActivePlayers.Count();
            float rotationAngle = playerCount % 2 == 0 ? leftDirectionAngle : rightDirectionAngle;
            Quaternion playerRotation = Quaternion.Euler(0, rotationAngle, 0);
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), playerRotation, player);
        }
    }
}