using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;
    public void ScreenShake(Vector2 dir)
    {
        impulseSource.GenerateImpulse(dir);
    }
}
