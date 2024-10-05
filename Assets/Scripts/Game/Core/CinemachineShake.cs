using UnityEngine;
using Unity.Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    public void ShakeCamera()
    {
        _impulseSource.GenerateImpulse();   
    }
}
