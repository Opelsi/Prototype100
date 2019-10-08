using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = 0.01f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(1, 5)] [SerializeField] private float m_SprintSpeed = 2f;          // Amount of maxSpeed applied to sprinting movement. 1 = 100%
	[Range(10, 50)] [SerializeField] private float m_DashSpeed = 10f;          // Amount of maxSpeed applied to dashing movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask m_WhatIsWall;                          // A mask determining what is wall to the character
	[SerializeField] private LayerMask m_WhatIsMoveable;                          // A mask determining what is movable to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_BehindCheck;                           // A position marking where to check if the player hanging on left wall.
	[SerializeField] private Transform m_FrontCheck;                           // A position marking where to check if the player hanging on right wall.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_WallHangingRadius = .1f;
	private bool m_WallHanging;
	private bool m_WallFront = false;
	private bool m_WallBehind = false;
	private bool m_MoveableFront = false;
	private bool m_MoveableBehind = false;
	const float k_MoveRadius = .2f;
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private GameObject objectToPush = null;
	private GameObject objectToPull = null;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent OnWallHangingEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;
	public BoolEvent OnSprintEvent;
	private bool m_wasSprinting = false;
	public BoolEvent OnDashEvent;
	private bool m_wasDashing = false;
	public BoolEvent OnPullingEvent;
	private bool m_wasPulling = false;
	public BoolEvent OnPushingEvent;
	private bool m_wasPushing = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		if (OnWallHangingEvent == null)
			OnWallHangingEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
		if (OnSprintEvent == null)
			OnSprintEvent = new BoolEvent();
		if (OnDashEvent == null)
			OnDashEvent = new BoolEvent();
		if (OnPullingEvent == null)
			OnPullingEvent = new BoolEvent();
		if (OnPushingEvent == null)
			OnPushingEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		m_WallHanging = false;
		m_WallBehind = false;
		m_WallFront = false;
		m_MoveableFront = false;
		m_MoveableBehind = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] groundColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < groundColliders.Length; i++)
		{
			if (groundColliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		Collider2D[] behindWallColliders = Physics2D.OverlapCircleAll(m_BehindCheck.position, k_WallHangingRadius, m_WhatIsWall);
		for (int i = 0; i < behindWallColliders.Length; i++)
		{
			if (behindWallColliders[i].gameObject != gameObject)
			{
				m_WallBehind = true;
			}
		}

		Collider2D[] frontWallColliders = Physics2D.OverlapCircleAll(m_FrontCheck.position, k_WallHangingRadius, m_WhatIsWall);
		for (int i = 0; i < frontWallColliders.Length; i++)
		{
			if (frontWallColliders[i].gameObject != gameObject)
			{
				m_WallFront = true;
			}
		}
		Collider2D[] pushableColliders = Physics2D.OverlapCircleAll(m_FrontCheck.position, k_MoveRadius, m_WhatIsMoveable);
		for (int i = 0; i < pushableColliders.Length; i++)
		{
			if (pushableColliders[i].gameObject != gameObject)
			{
				m_MoveableFront = true;
				objectToPush = pushableColliders[i].gameObject;
			}
		}
		Collider2D[] pullableColliders = Physics2D.OverlapCircleAll(m_BehindCheck.position, k_MoveRadius, m_WhatIsMoveable);
		for (int i = 0; i < pullableColliders.Length; i++)
		{
			if (pullableColliders[i].gameObject != gameObject)
			{
				m_MoveableBehind = true;
				objectToPull = pullableColliders[i].gameObject;
			}
		}
	}


	public void Move( float move, bool crouch, bool jump, bool jumpDown, bool sprint, bool dash)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			//If Sprinting
			if (sprint)
			{
				if (!m_MoveableBehind)
				{
					if (!m_wasSprinting)
					{
						m_wasSprinting = true;
						OnSprintEvent.Invoke(true);
					}
					// Increase the speed by the sprintSpeed multiplier
					if(!m_MoveableFront)move *= m_SprintSpeed;

					if (m_wasPulling)
					{
						m_wasPulling = false;
						OnPullingEvent.Invoke(false);
					}
					if (objectToPull)objectToPull = null;
				}
				else
				{
					if (m_wasSprinting)
					{
						m_wasSprinting = false;
						OnSprintEvent.Invoke(false);
					}
					move *= m_CrouchSpeed;

					if (m_Grounded)
					{
						if (!m_wasPulling)
						{
							m_wasPulling = true;
							OnPullingEvent.Invoke(true);
						}
						if (objectToPull)
						{
							objectToPull.GetComponent<Rigidbody2D>().velocity = m_Rigidbody2D.velocity;
						}
					}
				}
			}
			//If Dashing
			if (dash && !m_MoveableFront)
			{
				if (!m_wasDashing)
				{
					m_wasDashing = true;
					OnDashEvent.Invoke(true);
				}
				move *= m_DashSpeed;
				if (!m_FacingRight) move *= -1;
			}
			else
			{
				if (m_wasDashing)
				{
					m_wasDashing = false;
					OnDashEvent.Invoke(false);
				}
			}
			// If crouching
			if (!sprint && crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
		if (!m_WallHanging && m_WallFront && !m_WallBehind && !m_Grounded)
		{
			m_AirControl = false;
			m_WallHanging = true;
			m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
		}
		if(m_WallHanging)
		{
			if (jump || jumpDown)
			{
				Flip();
				m_AirControl = true;
				m_WallHanging = false;
				m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
				if(m_FacingRight)m_Rigidbody2D.AddForce(new Vector2(500f, 0f));
				else m_Rigidbody2D.AddForce(new Vector2(-500f, 0f));
				if (jump)m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce*1.25f));
			}
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}