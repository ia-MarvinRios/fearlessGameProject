using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public string nextScene = "";
    public int spawnPointIndex = 0;
    Collider _other;
    Gui3D gui3D;

    private void Start()
    {
        gui3D = FindObjectOfType<Gui3D>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            _other = other;
        }

    }
    public void Einteract()
    {
        if (gui3D.IsTooltipActive && _other != null && nextScene != "")
        {
            playerInfo.SpawnPointIndex = spawnPointIndex;
            playerInfo.LastPosition = _other.transform.position;
            playerInfo.LastRotation = _other.transform.rotation;
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _other = null;
        }
    }
}

[CreateAssetMenu]
public class PlayerInfo : ScriptableObject
{
    private Vector3 lastPosition;
    public Vector3 LastPosition { get { return lastPosition; } set { lastPosition = value; } }
    private Quaternion lastRotation;
    public Quaternion LastRotation { get { return lastRotation; } set { lastRotation = value; } }

    [SerializeField] private int spawnPointIndex = 0;
    public int SpawnPointIndex { get { return spawnPointIndex; } set { spawnPointIndex = value; } }

    public Vector3[] spawnPoints;
}