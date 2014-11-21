using UnityEngine;
using System.Collections;

//perform moving cells
public class MoveCell : State {
	public MoveCell(string name):base(name){}

	float currTime = 0f;

	public override void EnterState(FSM fsm)
	{
		currTime = 0f;

		DBG.Log("Enter MoveCell");
	}
	
	public override State RunState(FSM fsm)
	{
		currTime += Time.deltaTime;

		if(currTime >= GameConfig.CellMoveTime)
			return m_nextStates["Upgrade"];

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		DBG.Log("Leave MoveCell");
	}
}
