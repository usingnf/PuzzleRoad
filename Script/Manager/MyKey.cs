using UnityEngine;

/// <summary>
/// 사용자 정의 Key 값.
/// </summary>

public enum KeyType
{
    Move,
    Stop,
    Interact,
    Light,
    CameraZ,
    CameraC,
    Walk,
}
public static class MyKey
{
    public static KeyCode Move = KeyCode.Mouse1;
    public static KeyCode Stop = KeyCode.S;
    public static KeyCode Interact = KeyCode.E;
    public static KeyCode Light = KeyCode.F;
    public static KeyCode CameraZ = KeyCode.Z;
    public static KeyCode CameraC = KeyCode.C;
    public static KeyCode Escape = KeyCode.Escape;
    public static KeyCode Reset = KeyCode.R;
    public static KeyCode Walk = KeyCode.Q;
}
