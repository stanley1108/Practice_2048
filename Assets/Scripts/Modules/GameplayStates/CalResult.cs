using UnityEngine;
using System.Collections;

public class CalResult : State {
	public CalResult(string name):base(name){}

	//for counting combo
	int lastScore = 0;

	public override void EnterState(FSM fsm)
	{
		int currScore = GameCore.Instance.CurrScore;

		if(CellMap.Instance.IsReadyUpgradeCell == true)
		{
			GameCore.Instance.ResetComboCount();
		}
		else if(currScore > lastScore)
		{
			lastScore = currScore;
			GameCore.Instance.AddComboCount();
		}
		
		CellMap.Instance.IsReadyUpgradeCell = false;

		DBG.Log("Enter CalResult");
	}
	
	public override State RunState(FSM fsm)
	{


		if(CellMap.Instance.IsMoveable() == false)
		{
			GameCore.Instance.LoseGame();
			lastScore = 0;
		}


		if(CellMap.Instance.Is2048())
		{
			GameCore.Instance.WinGame();
			lastScore = 0;
		}

		return m_nextStates["WaitInput"];
	}
	
	public override void LeaveState(FSM fsm)
	{
		DBG.Log("Leave CalResult");
	}
}
