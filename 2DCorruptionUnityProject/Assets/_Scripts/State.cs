using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class State
{
    public enum STATE
    {
        IDLE, JUMP, DASH, ATTACK, PURSUE, PATROL
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject enemy;
    protected Animator anim;
    protected PlayerInputActions playerInputActions;
    protected Skills playerSkills;
    protected State nextState;

    float visionDistance = 10.0f;
    float projectileDistance = 7.0f;

    public State(Animator anim, PlayerInputActions playerInputActions, Skills playerSkills)
    {
        this.anim = anim;
        this.playerInputActions = playerInputActions;
        this.playerSkills = playerSkills;
        this.stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER)
            Enter();
        if (stage == EVENT.UPDATE)
            Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}

public class Idle : State
{
    public Idle(Animator anim, PlayerInputActions playerInputActions, Skills playerSkills)
        : base(anim, playerInputActions, playerSkills)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        // anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        if (playerInputActions.Player.Jump.WasPressedThisFrame())
        {
            nextState = new Jump(anim, playerInputActions, playerSkills);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Jump : State
{
    public Jump(Animator anim, PlayerInputActions playerInputActions, Skills playerSkills)
        : base(anim, playerInputActions, playerSkills)
    {
        name = STATE.JUMP;
        // What else goes here?
    }

    public override void Enter()
    {
        // anim.SetTrigger("isJumping");
        base.Enter();
    }

    public override void Update()
    {
        playerInputActions.Player.Jump.performed += playerSkills.Jump_performed;
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isJumping");
        base.Exit();
    }
}
