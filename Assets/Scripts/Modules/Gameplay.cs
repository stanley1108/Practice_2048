using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gameplay : State {
	public Gameplay(string name):base(name){}


	private FSM gameplayFSM_ = null;


	
	public override void EnterState(FSM fsm)
	{
		Debug.Log("Enter Gameplay");

		if(gameplayFSM_ != null)
			return;


	}
	
	public override State RunState(FSM fsm)
	{

		if(InputManager.IsSwipeRight())
		{
			Debug.Log("Change to next state");
			return m_nextStates["Rank"];
		}

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		Debug.Log("Leave Gameplay");
	}
}
