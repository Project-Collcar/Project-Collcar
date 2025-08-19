using Fusion;
using UnityEngine;

// 네트워크 입력 구조체
public struct NetworkInputData : INetworkInput
{
    public float steerInput;
    public float throttleInput;
    public bool handBrake;
    public bool skill1;
    public bool skill2;
}