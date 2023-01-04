using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    public enum State {IDLE, JUMP, DASH, ATTACK};

    public enum Event {ENTER, UPDATE, EXIT};

    public State playerState;
    protected Event playerEvent;
    protected Animator anim;
    protected PlayerInputActions playerInputActions;
    protected PlayerSkills playerSkills;
    protected PlayerState nextState;

    public PlayerState(Animator anim, PlayerInputActions playerInputActions, PlayerSkills playerSkills)
    {
        this.anim = anim;
        this.playerInputActions = playerInputActions;
        this.playerSkills = playerSkills;
        this.playerEvent = Event.ENTER;
    }

    public virtual void Enter() { playerEvent = Event.UPDATE; }
    public virtual void Update() { playerEvent = Event.UPDATE; }
    public virtual void Exit() { playerEvent = Event.EXIT; }

    //public State Process()
    public void Process()
    {
        if (playerEvent == Event.ENTER)
            Enter();
        if (playerEvent == Event.UPDATE)
            Update();
        if (playerEvent == Event.EXIT)
        {
            Exit();
            //return nextState;
        }
        //return this;
    }
}

public class Idle : PlayerState
{
    public Idle(Animator anim, PlayerInputActions playerInputActions, PlayerSkills playerSkills)
        : base(anim, playerInputActions, playerSkills)
    {
        playerState = State.IDLE;
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
            //stage = EVENT.EXIT;
            base.Exit();
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Jump : PlayerState
{
    public Jump(Animator anim, PlayerInputActions playerInputActions, PlayerSkills playerSkills)
        : base(anim, playerInputActions, playerSkills)
    {
        playerState = State.JUMP;
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
