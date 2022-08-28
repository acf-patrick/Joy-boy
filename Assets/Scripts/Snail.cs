using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : EnemyBehaviour
{
    public float playerPush = 10f;

    protected override void collisionLeft(RaycastHit2D hit)
    {
        if (state == State.Stomped)
            speed = playerPush;
        else
            Player.kill();
    }

    protected override void collisionRight(RaycastHit2D hit)
    {
        if (state == State.Stomped)
            speed = -playerPush;
        else
            Player.kill();
    }

    protected override void onStomp()
    {
        // Disable default behaviour
    }
}