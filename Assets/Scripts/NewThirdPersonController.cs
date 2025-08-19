using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class NewThirdPersonController : MonoBehaviour
{
    //private playerItemController playerItem;
    [Header("Player Atributes")]
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.33f;
    //things we might not need
    public float RotationSmoothTime = 0.12f;
    public float Acceleration = 10.0f;


    public AudioClip LandingAudioClip; // from starter assets
    public AudioClip[] FootstepAudioClips;// also from starter assets
    public float AudioVolume = 0.5f;

    public float Sensitivity = 1.0f;

    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    [Header("Camera")]
    public GameObject CameraTarget;
    public float TopClamp;
    public float BottomClamp;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;

    [Header("Collider")]
    // store not crouch values
    public Vector3 StandingCenter = new Vector3(0f, 0.93f, 0f);
    public float StandingHeight = 1.8f;
    public Vector3 CrouchingCenter = new Vector3(0f, 0.7f, 0f);
    public float CrouchingHeight = 1.35f;

    [Header("Weapons")]
    public GameObject Rifle;
    public GameObject Pistol;
    public GameObject Shotgun;
    public bool brian;

    [Header("Death")]
    public bool dead;

    public float zombieHitCoolDown = 1.0f;
    private float lastHitTime = -1f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private bool _rotateOnMove = true;

    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private PlayerInput _playerInput;

    private Animator _animator;
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    private float TimeToDie = 0f;


    private void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

    }

    private void Start()
    {
        //get our components
        _cinemachineTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        //_playerInput = GetComponent<PlayerInput>();
        //playerItem = GetComponent<playerItemController>();
        // if (playerItem == null)
        // {
        //     Debug.Log("PlayerItemController not found");
        // }
        // Debug.Log("PlayerItemController found"+playerItem.rifleAmmo);

        // initialize our jump Deltas (we may not use this)
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            _animator.SetBool("Dead", true);
            TimeToDie += Time.deltaTime;
            if (TimeToDie > 5f)
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            // for crouching
            UpdateHitbox();

            // set the cameraTarget to our _controller center 
            float offSet = _input.crouch ? .3f : 1f;
            Vector3 camTarget = new Vector3(CameraTarget.transform.position.x, _controller.transform.position.y + offSet, CameraTarget.transform.position.z);
            CameraTarget.transform.position = camTarget;

            JumpAndGravity();
            GroundedCheck();
            Move();

        }

        if (playerItemController.instance.playerHealth <= 0) {
            // Debug.Log("Player look lethal damage down to " + playerItemController.instance.playerHealth);
            dead = true;
        }

    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void UpdateHitbox()
    {
        // update cameara root and hit box 
        if (_input.crouch)
        {
            _controller.center = CrouchingCenter;
            _controller.height = CrouchingHeight;
        }
        else
        {
            _controller.center = StandingCenter;
            _controller.height = StandingHeight;
        }
    }

    private void CameraRotation()
    {
        // this is camera rotation stuff from the starter assets controller
        _cinemachineTargetPitch += _input.look.y *  Sensitivity;
        _cinemachineTargetYaw += _input.look.x * Sensitivity;

        // make sure we cant rotate past 360
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);

        // now we have the rotation of our camera
        CameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);

    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void Move()
    {
        float targetSpeed = GetTargetSpeed();


        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        SetSpeed(targetSpeed, inputMagnitude);

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * Acceleration);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        if (_input.move != Vector2.zero) RotatePlayer();

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update our animator
        UpdateAnimator(inputMagnitude);

        // crouching 

    }

    private void UpdateAnimator(float inputMagnitude)
    {
        // animation for not aiming 
        _animator.SetFloat("Speed", _animationBlend);
        _animator.SetFloat("MotionSpeed", inputMagnitude);

        //animation for aiming
        _animator.SetFloat("Y_Movement", _input.move.y);
        _animator.SetFloat("X_Movement", _input.move.x);

        _animator.SetBool("Crouch", _input.crouch);
        _animator.SetBool("Aim", _input.aim);

        
        if (_input.weapon2)
        {
            _animator.SetBool("Pistol", false);
            Rifle.SetActive(true);
            Pistol.SetActive(false);
            Shotgun.SetActive(false);

        }
        else if(_input.weapon3 && !brian)
        {
            _animator.SetBool("Pistol", false);
            Rifle.SetActive(false);
            Pistol.SetActive(false);
            Shotgun.SetActive(true);
        }
        else 
        {
            _animator.SetBool("Pistol", true);
            Rifle.SetActive(false);
            Pistol.SetActive(true);
            Shotgun.SetActive(false);
            _input.weapon3 = false; 
        }

    }
    private void RotatePlayer()
    {
        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                            _mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            RotationSmoothTime);
        if (_rotateOnMove)
        {
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        }
    }

    private void SetSpeed(float targetSpeed, float inputMagnitude)
    {
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * Acceleration);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
    }

    private float GetTargetSpeed()
    {
        float targetSpeed;
        if (!_input.aim)
        {
            targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
        }
        else
        {
            targetSpeed = MoveSpeed;
        }

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        return targetSpeed;
    }


    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        _animator.SetBool("Grounded", Grounded);
    }

    private void JumpAndGravity()
    {
        // PERSONAL NOTE - MAX: This is from the starter assets controller.
        // There is also a rigid body script that allows the character to push things
        // If we want to use a rigid body for grav instead of this let me know
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            _animator.SetBool("Jump", false);
            _animator.SetBool("FreeFall", false);
           
            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square 1 of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                _animator.SetBool("Jump", true);     
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
                _animator.SetBool("FreeFall", true);
            }

            // if we are not grounded, do not jump
            _input.jump = false;
        }
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), AudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), AudioVolume);
        }
    }

    public void SetSensitivity(float newSensitivity)
    {
        Sensitivity = newSensitivity;
    }

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        _rotateOnMove = newRotateOnMove;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Zombie" && !hit.gameObject.GetComponent<ZombieController>().calledDeath)
        {
            //dead = true;
            if (Time.time >= lastHitTime + zombieHitCoolDown)
            {
                playerItemController.instance.playerHealth -= 4;
                if (playerItemController.instance.playerHealth <= 0)
                {
                    dead = true;
                }
                lastHitTime = Time.time;
            }
        }
    }
}
