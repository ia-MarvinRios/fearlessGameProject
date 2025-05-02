using UnityEngine;

public class PlayerWorldPlacer : MonoBehaviour
{
    public void PlaceBySpawnPointID(PlayerInfo playerInfo)
    {
        if (playerInfo != null)
        {
            transform.position = playerInfo.spawnPoints[playerInfo.SpawnPointIndex];
            playerInfo.SpawnPointIndex = 0;
        }
        else
        {
            Debug.LogWarning("playerInfo no existe o no es válido.");
        }
    }
}