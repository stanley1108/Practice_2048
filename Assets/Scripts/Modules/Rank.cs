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
	}
	
	public override State RunState(FSM fsm)
	{
		currTime += Time.deltaTime;
		
		if(currTime >= coundownTime)
		{
			Debug.Log("Change to next state");
			return m_nextStates["Gameplay"];
		}

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		Debug.Log("Leave Rank");
	}
}
