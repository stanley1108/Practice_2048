using UnityEngine;
using System.Collections;

public class Upgrade : State {
	public Upgrade(string name):base(name){}

	float currTime = 0f;
	float waitTime = 0f;
	
	public override void EnterState(FSM fsm)
	{
		currTime = 0f;
		waitTime = GameConfig.CellBeginTime;

		DBG.Log("Enter Upgrade");

		if(CellMap.Instance.IsReadyUpgradeCell == true)
		{
		//	GameCore.Instance.ResetComboCount();
			CellMap.Instance.UpgradCells();
		}
	}
	
	public override State RunState(FSM fsm)
	{		
		currTime += Time.deltaTime;

		if(currTime >= GameConfig.CellBeginTime)
			return m_nextStates["GenerateCell"];

		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{

		DBG.Log("Leave Upgrade");
	}
}
