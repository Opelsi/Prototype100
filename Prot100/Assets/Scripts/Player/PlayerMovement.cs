using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController2D controller;
	float runSpeed = 40f;
	public Animator animator;
	bool isJump = false;
	bool isJumpDown = false;
	bool isSprint = false;
	bool isDash = false;
	bool isCrouch = false;
	bool isFall = false;
	float horizontalMove = 0f;


	bool dashPressed = false;
	bool reset = false;
	float timeOfFirstButton = 0f;

	void Update()
	{
		//Sprinting
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			if(!isFall && !isJump) isSprint = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			isSprint = false;
		}
		//SettingMove
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		//Dashing
		if (Input.GetKeyDown(KeyCode.LeftControl) && !isDash)
		{
			isDash = true;
			horizontalMove = runSpeed;
		}
		//Jumping
		if (Input.GetButtonDown("Jump") && transform.GetComponent<Rigidbody2D>().velocity.y < 0.1)
		{
			isJump = true;
			animator.SetBool("IsJump", true);
		}
		//Crouching
		if (Input.GetButtonDown("Crouch"))
		{
			isCrouch = true;
			isJumpDown = true;
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			isCrouch = false;
		}
		isFall = (transform.GetComponent<Rigidbody2D>().velocity.y < -1);
		animator.SetBool("IsFall", isFall);
		if(isFall) animator.SetBool("IsJump", false);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJump", false);
		isFall = false;
	}

	public void OnCrouching( bool isCrouching )
	{
		animator.SetBool("IsCrouch", isCrouching);
	}

	private void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.deltaTime, isCrouch, isJump, isJumpDown, isSprint, isDash);
		isJump = false;
		isDash = false;
		isJumpDown = false;
	}
}
