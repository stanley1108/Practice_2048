using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	public int currPos_X = 0;
	public int currPos_Y = 0;
	private Vector3 currPosition_ = Vector3.zero;

	public int targetPos_X = 0;
	public int targetPos_Y = 0;
	private Vector3 targetPosition_ = Vector3.zero;

	public enum State
	{
		Idle,
		Moving,
		MovingToDie,
	}

	public State state_ = State.Idle;

	public enum Level
	{
		Num_2,
		Num_4,
		Num_8,
		Num_16,
		Num_32,
		Num_64,
		Num_128,
		Num_256,
		Num_512,
		Num_1024,
		Num_2048,

		Count
	}
	SpriteRenderer spriteRenderer_;
	private Level currLevel_ = Level.Num_2;
	public Level CurrLevel
	{
		get{return currLevel_;}
		set
		{
			currLevel_ = value;

			//change sprite
			spriteRenderer_.sprite = GameCore.Instance.GetCellSprite((int)currLevel_);
		}
	}


	[SerializeField]
	float totalMoveTime = 1f;

	float currMoveTime = 0f;


	void Awake () {
		spriteRenderer_ = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(state_)
		{
		case State.Idle:
			break;
		case State.Moving:
			currMoveTime += Time.deltaTime;
			if(currMoveTime >= totalMoveTime)
			{
				currMoveTime = 0;
				state_ = State.Idle;

				transform.position = targetPosition_;
				currPosition_ = targetPosition_;
				currPos_X = targetPos_X;
				currPos_Y = targetPos_Y;
				break;
			}


			transform.position = Vector3.Lerp(currPosition_, targetPosition_, currMoveTime / totalMoveTime);

			break;
		case State.MovingToDie:
			currMoveTime += Time.deltaTime;
			if(currMoveTime >= totalMoveTime)
			{
				currMoveTime = 0;

				state_ = State.Idle;
				GameCore.Instance.RecycleCell(this);
				break;
			}

			transform.position = Vector3.Lerp(currPosition_, targetPosition_, currMoveTime / totalMoveTime);
			break;
		}

	}

	public void SetCurrPos(int x, int y)
	{

		float size = GameCore.Instance.cellSize_;
		currPosition_.x = x * size;
		currPosition_.y = -y * size;
		transform.position = currPosition_;

		currPos_X = x;
		currPos_Y = y;
	}

	public void SetTargetPos(int x, int y)
	{
		
		float size = GameCore.Instance.cellSize_;
		targetPosition_.x = x * size;
		targetPosition_.y = -y * size;
		
		targetPos_X = x;
		targetPos_Y = y;
	}
}
