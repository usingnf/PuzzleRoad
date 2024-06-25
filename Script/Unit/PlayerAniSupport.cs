using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션 KeyFrame Event와 연결됨.
/// </summary>
public class PlayerAniSupport : MonoBehaviour
{
    [SerializeField] private PlayerUnit player;
    public void RunSound1(float volume)
    {
        player.MoveSound(1, volume);
    }

    public void RunSound2(float volume)
    {
        player.MoveSound(2, volume);
    }

    public void WalkSound1(float volume)
    {
        player.MoveSound(3, volume);
    }
    public void WalkSound2(float volume)
    {
        player.MoveSound(4, volume);
    }
}
