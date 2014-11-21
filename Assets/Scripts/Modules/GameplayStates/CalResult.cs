using UnityEngine;
using System.Collections;

public class CalResult : State {
	public CalResult(string name):base(name){}
	
	public override void EnterState(FSM fsm)
	{
		Debug.Log("Enter CalResult");
	}
	
	public override State RunState(FSM fsm)
	{
		if(CellMap.Instance.IsMoveable() == false)
			GameCore.Instance.LoseGame();


		if(CellMap.Instance.Is2048())
			GameCore.Instance.WinGame();

		return m_nextStates["WaitInput"];
	}
	
	public override void LeaveState(FSM fsm)
	{
		Debug.Log("Leave CalResult");
	}
}
