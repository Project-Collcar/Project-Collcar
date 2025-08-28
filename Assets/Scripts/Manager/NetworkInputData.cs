using Fusion;
using UnityEngine;

public enum InputButtons
{
    SKILL1 = 0,
    SKILL2 = 1
}

// 네트워크 입력 구조체
public struct NetworkInputData : INetworkInput
{
    public float steerInput;
    public float throttleInput;
    public bool handBrake;
    public NetworkButtons Buttons;
}