using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    int combCounter;
    float lastTimeAttacked;
    float combWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        if (combCounter > 2 || Time.time > lastTimeAttacked + combWindow)
            combCounter = 0;

        player.anim.SetInteger("CombCounter", combCounter);
        this.stateTimer = 0.1f;

        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[combCounter].x * attackDir, player.attackMovement[combCounter].y);


    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            this.stateMachine.ChangeState(player.idleState);

        if (this.stateTimer < 0)
            player.SetVelocityZero();


    }
    public override void Exit()
    {
        combCounter++;
        base.Exit();
        lastTimeAttacked = Time.time;
        player.StartCoroutine("BusyFor", 0.17f);
    }

}
