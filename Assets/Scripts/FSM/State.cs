using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class State {
	protected string m_name = "none";
	public string Name
	{
		get{return m_name;}	
	}
	
	protected State()
	{
	}
	
	public State(string name)
	{
		m_name = name;
	}
	
	
	protected Dictionary<string, State> m_nextStates = new Dictionary<string, State>();
	
	public void AddNextState(string name, State next) { m_nextStates.Add(name, next);}
	
	
	
	
	public abstract void EnterState(FSM fsm);
	
	public abstract State RunState(FSM fsm);
	
	public abstract void LeaveState(FSM fsm);
}
