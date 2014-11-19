using UnityEngine;
using System.Collections;

//wait inputs like swipe, open menu...etc 
public class WaitInput : State {
	public WaitInput(string name):base(name){}
	
	public override void EnterState(FSM fsm)
	{
		
	}
	
	public override State RunState(FSM fsm)
	{
		return null;
	}
	
	public override void LeaveState(FSM fsm)
	{

	}
}
