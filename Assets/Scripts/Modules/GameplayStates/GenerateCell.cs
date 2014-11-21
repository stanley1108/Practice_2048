using UnityEngine;
using System.Collections;

public class GenerateCell : State {
	public GenerateCell(string name):base(name){}

	float currTime = 0f;

	public override void EnterState(FSM fsm)
	{
		currTime = 0f;

		Debug.Log("Enter GenerateCell");

		CellMap.Instance.RandomGenerateCell();
	}
	
	public override State RunState(FSM fsm)
	{
		currTime += Time.deltaTime;
		
		if(currTime >= GameConfig.CellBeginTime)
			return m_nextStates["CalResult"];

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{
		Debug.Log("Leave GenerateCell");
	}
}
