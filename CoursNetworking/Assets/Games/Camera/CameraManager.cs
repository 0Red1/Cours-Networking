using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    
    [SerializeField] private Camera camera;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    public void Awake()
    {
        Instance = this;
    }
    
    public void RegisterPlayer(Transform player, bool isMainPlayer)
    {
        float weight = isMainPlayer ? 2f : 1f; 
        float radius = 2f;

        targetGroup.AddMember(player, weight, radius);
    }
}
