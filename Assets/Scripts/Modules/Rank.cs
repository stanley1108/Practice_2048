using UnityEngine;
using System.Collections;

public class Rank : State {
	public Rank(string name):base(name){}

	//test
	float currTime = 0;
	float coundownTime = 3f;

	public override void EnterState(FSM fsm)
	{
		currTime = 0;

		Debug.Log("Enter Rank");

		if(GameCore.Instance.IsWin)
			Debug.Log("Game win!!!!!!!!!!!!!!!!!!!!!!");
		else
			Debug.Log("Game lose!!!!!!!!!!!!!!!!!!!!!!");
	}
	
	public override State RunState(FSM fsm)
	{
		if(InputManager.IsPressAnyKey())
		{
			return m_nextStates["Gameplay"];
		}

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		GameCore.Instance.RestartGame();

		Debug.Log("Leave Rank");
	}
}
