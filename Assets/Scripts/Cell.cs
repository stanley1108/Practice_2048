using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
	public int currPos_X = 0;
	public int currPos_Y = 0;
	private Vector3 currPosition_ = Vector3.zero;

	public int targetPos_X = 0;
	public int targetPos_Y = 0;
	private Vector3 targetPosition_ = Vector3.zero;

	private float originScale_ = 1f;
	private Vector3 currScaleVec_ = Vector3.zero;

	public enum State
	{
		Begin,
		Idle,
		Moving,
		MovingToMerge,
		MovingToDie,
	}

	public State state_ = State.Begin;

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

	[SerializeField]
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

	float currStateTime = 0f;

	void Awake () {
		spriteRenderer_ = GetComponent<SpriteRenderer>();

		originScale_ = transform.localScale.x;
		currScaleVec_ = new Vector3(originScale_, originScale_, 1);
	}
	
	// Update is called once per frame
	void Update () {
		switch(state_)
		{
		case State.Begin:
			currStateTime += Time.deltaTime;
			if(currStateTime >= GameConfig.CellBeginTime)
			{
				currScaleVec_.x = originScale_;
				currScaleVec_.y = originScale_;
				transform.localScale = currScaleVec_;

				currStateTime = 0;
				state_ = State.Idle;
				break;
			}
			float newScale = 2f * currStateTime / GameConfig.CellBeginTime;
			if(newScale > 1)
				newScale = 2f - newScale;

			newScale = originScale_ + originScale_ * (GameConfig.CellMaxScale - 1f) * newScale;
			currScaleVec_.x = newScale;
			currScaleVec_.y = newScale;

			transform.localScale = currScaleVec_;
			break;
		case State.Idle:
			break;
		case State.Moving:
			currStateTime += Time.deltaTime;
			if(currStateTime >= GameConfig.CellMoveTime)
			{
				currStateTime = 0;
				state_ = State.Idle;

				transform.position = targetPosition_;
				currPosition_ = targetPosition_;
				currPos_X = targetPos_X;
				currPos_Y = targetPos_Y;
				break;
			}
			transform.position = Vector3.Lerp(currPosition_, targetPosition_, currStateTime / GameConfig.CellMoveTime);

			break;
		case State.MovingToMerge:
			currStateTime += Time.deltaTime;
			if(currStateTime >= GameConfig.CellMoveTime)
			{
				currStateTime = 0;
				state_ = State.Begin;
				
				transform.position = targetPosition_;
				currPosition_ = targetPosition_;
				currPos_X = targetPos_X;
				currPos_Y = targetPos_Y;

				float plusScore = Mathf.Pow(2f,(float)currLevel_ + 1f);
				GameCore.Instance.AddScore((int)plusScore);
				break;
			}
			transform.position = Vector3.Lerp(currPosition_, targetPosition_, currStateTime / GameConfig.CellMoveTime);
			
			break;
		case State.MovingToDie:
			currStateTime += Time.deltaTime;
			if(currStateTime >= GameConfig.CellMoveTime)
			{
				currStateTime = 0;

				state_ = State.Idle;
				CellMap.Instance.RecycleCell(this);
				break;
			}

			transform.position = Vector3.Lerp(currPosition_, targetPosition_, currStateTime / GameConfig.CellMoveTime);
			break;
		}

	}

	public void SetCurrPos(int x, int y)
	{

		float size = GameConfig.CellSize;
		currPosition_.x = x * size;
		currPosition_.y = -y * size;
		transform.position = currPosition_;

		currPos_X = x;
		currPos_Y = y;
	}

	public void SetTargetPos(int x, int y)
	{
		
		float size = GameConfig.CellSize;
		targetPosition_.x = x * size;
		targetPosition_.y = -y * size;
		
		targetPos_X = x;
		targetPos_Y = y;
	}
}
