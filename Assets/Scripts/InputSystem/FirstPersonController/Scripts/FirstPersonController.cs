using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 3.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.5f;
        [Tooltip("Crouch speed of the character in m/s")]
        public float CrouchSpeed = 1.5f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		public bool analogMovement = true;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

        [Header("Interactions")]
        public LayerMask interactableLayerMask;
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);

        // cinemachine
        private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
        private float _animationBlend;

        private bool _isDancing = false;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        // timeout deltatime
        private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// Animation IDs
		private int _animIDSpeed;
        private int _animIDMotionSpeed;
		private int _animIDCrouching;
		private int _animIDJump;
		private int _animIDFreefall;
		private int _animIDGrounded;
        private int _animIDTwerking;
		private int _animIDHipHop;
		private int _animIDLeftHandActive;

		// Inputs
		Vector2 _lookInput = Vector2.zero;
		Vector2 _moveInput = Vector2.zero;
		float _jumpInput = 0;
		float _crouchInput = 0;
		float _sprintInput = 0;
		internal bool _canLook = true;

		[SerializeField] GuiManager guiMan;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
		private Animator _animator;
		private CharacterController _controller;
		private GameObject _mainCamera;

        private bool _hasAnimator;

        private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}

			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Start()
		{
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			GetInputs();
			JumpAndGravity();
			GroundedCheck();
			Move();
			Crouch();
			CheckRaycast();
			StopExtras();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			_animIDCrouching = Animator.StringToHash("Crouching");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreefall = Animator.StringToHash("Freefall");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDTwerking = Animator.StringToHash("Twerking");
			_animIDHipHop = Animator.StringToHash("HipHop");

			_animIDLeftHandActive = Animator.StringToHash("LeftHandActive");
        }

        private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

		private void GetInputs()
		{
			_lookInput = _playerInput.actions["Look"].ReadValue<Vector2>();
            _moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
			_jumpInput = _playerInput.actions["Jump"].ReadValue<float>();
			_crouchInput = _playerInput.actions["Crouch"].ReadValue<float>();
			_sprintInput = _playerInput.actions["Sprint"].ReadValue<float>();
        }

		private void CameraRotation()
		{
			// if there is an input
			if (_lookInput.sqrMagnitude >= _threshold && _canLook == true)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _lookInput.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _lookInput.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed;

            if (_crouchInput != 0)
			{
                targetSpeed = CrouchSpeed;
            }
            else if (_sprintInput != 0)
			{
                targetSpeed = SprintSpeed;
            }
			else
			{
                targetSpeed = MoveSpeed;
            }

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_moveInput == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = analogMovement ? _moveInput.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            // normalise input direction
            Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_moveInput != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			//Debug.Log(inputDirection);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void Crouch()
        {
            if (_hasAnimator)
            {
                if (_crouchInput != 0)
				{
					_animator.SetBool(_animIDCrouching, true);
				}
				else if (_crouchInput == 0)
				{
					_animator.SetBool(_animIDCrouching, false);
				}
            }
        }
		
		public void Twerk(InputAction.CallbackContext callbackContext)
		{
			if (_hasAnimator && callbackContext.performed)
			{
                _isDancing = true;
                _animator.SetBool(_animIDHipHop, false);

                UnlockModel();

                _animator.SetBool(_animIDTwerking, true);
            }
		}

        public void HipHop(InputAction.CallbackContext callbackContext)
        {
            if (_hasAnimator && callbackContext.performed)
            {
                _isDancing = true;
                _animator.SetBool(_animIDTwerking, false);
                UnlockModel();

                _animator.SetBool(_animIDHipHop, true);
            }
        }

		private void StopExtras()
		{
            if (_isDancing == true && (!Grounded || _moveInput != Vector2.zero))
			{
                _animator.SetBool(_animIDTwerking, false);
                _animator.SetBool(_animIDHipHop, false);

                RestoreModel();
                _isDancing = false;
            }
        }
        private void RestoreModel()
		{
            _animator.applyRootMotion = false;
            transform.position = _originalPosition;
            transform.rotation = _originalRotation;
        }
		private void UnlockModel()
		{
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
            _animator.applyRootMotion = true;
        }
		
        private void JumpAndGravity()
		{
            if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
                    _animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreefall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_jumpInput != 0 && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreefall, true);
                    }
                }

                // if we are not grounded, do not jump
                _jumpInput = 0;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		public void PauseInput(InputAction.CallbackContext callbackContext)
		{
			if (callbackContext.performed)
			{
				guiMan.PauseGame();
			}
		}

        public void InteractionInput(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
            {
                InteractionCaseExecuter(CheckRaycast());
            }
            if (callbackContext.canceled)
            {
            }
        }
        private RaycastHit CheckRaycast()
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, 6f, interactableLayerMask))
			{
				guiMan.ShowToolTips(hit);
				return hit;
			}
			else
			{
				guiMan.HideToolTips();
				return default(RaycastHit);
			}
		}
        private void InteractionCaseExecuter(RaycastHit hit)
        {
            if (hit.transform == null) return;

            if (hit.transform.gameObject.CompareTag("Door"))
            {
                Vector3 currentRot = hit.transform.rotation.eulerAngles;
                if (currentRot.y == 0)
                    hit.transform.Rotate(new Vector3(0, 90, 0));
                else if (currentRot.y == 90)
                    hit.transform.Rotate(new Vector3(0, -90, 0));
                else if (currentRot.y == 180)
                    hit.transform.Rotate(new Vector3(0, -90, 0));
                else if (currentRot.y == 270)
                    hit.transform.Rotate(new Vector3(0, -90, 0));
            }
            if (hit.transform.gameObject.CompareTag("NPC") && hit.transform.GetComponent<NPC>().availableForDialogue == true)
            {
                StartCoroutine(hit.transform.GetComponent<NPC>().StartDialogue());
				hit.transform.rotation = Quaternion.LookRotation(transform.position - hit.transform.position);
            }
			if (hit.transform.gameObject.CompareTag("Hacha"))
			{

			}
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}