using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
	public Transform target;

	public float speed = 200f;
	public float nextWaypointDistance = 3f;
	public float attackRange = 2f;

	public bool isFollow = false;
	Vector2 direction;

	Path path;
	int currentWaypoint = 0;
	bool reachedEndOfPath = false;

	Seeker seeker;
	Rigidbody2D rb;
	float jumpTimer = 0f;

	CharacterController2D controller;
	public Animator enemyAnimator;
	bool isJump = false;
	bool isJumpDown = false;
	bool isSprint = false;
	bool isDash = false;
	bool isCrouch = false;
	bool isFall = false;

	// Start is called before the first frame update
	void Start()
    {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		controller = GetComponent<CharacterController2D>();

		InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

	void UpdatePath()
	{
		if(seeker.IsDone())
			seeker.StartPath(rb.position, target.position, OnPathComplete);
	}

	void OnPathComplete(Path p )
	{
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}

	public bool TargetIsFar()
	{
		return ((target.position - transform.position).magnitude > attackRange);
	}

	void FollowTarget()
	{
		direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

		if (direction.x > 0.1f)
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if (direction.x < 0.1f)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}

		float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
		if (distance < nextWaypointDistance)
		{
			currentWaypoint++;
			enemyAnimator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
		}

		if (Mathf.Abs(rb.velocity.y) < 0.1f && direction.y > 0.7f && Time.time - jumpTimer > 2f)
		{
			isJump = true;
			jumpTimer = Time.time;
		}

		isFall = (rb.velocity.y < -0.1f);
		if (isFall) isJump = false;
		enemyAnimator.SetBool("IsJump", isJump);
		enemyAnimator.SetBool("IsFall", isFall);

		Vector2 force = direction * speed * Time.deltaTime;
		controller.Move(force.x, false, isJump, false, false, false);
		isJump = false;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (path == null) return;

		if (currentWaypoint >= path.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		else
		{
			reachedEndOfPath = false;
		}

		if (isFollow) FollowTarget();
	}
}
