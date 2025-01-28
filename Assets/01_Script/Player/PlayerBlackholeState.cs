using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    float flyTime = .4f;
    bool skillUsed;
    float defaultGravity;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        stateTimer = flyTime;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
        
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, -.1f);
            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }

            }
        }
        if (player.skill.blackhole.SkillCompleted()) {
            stateMachine.ChangeState(player.airState);
        }

    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.fx.MakeTransprent(false);
    }
}
