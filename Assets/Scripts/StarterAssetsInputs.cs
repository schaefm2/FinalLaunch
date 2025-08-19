using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool shoot;
		public bool crouch;
		public bool weapon1;
		public bool weapon2;
		public bool weapon3;
		public bool rifleShoot;
		public bool grenade;
		public bool heal;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
        public void OnCrouch(InputValue value)
        {
            CrouchInput(value.isPressed);
        }

		public void OnWeapon1(InputValue value)
		{
			Weapon1Input(value.isPressed);
		}

        public void OnWeapon2(InputValue value)
        {
            Weapon2Input(value.isPressed);
        }

		public void OnWeapon3(InputValue value)
		{
			Weapon3Input(value.isPressed);
		}

		public void OnRifleShoot(InputValue value)
		{
			RifleShoot(value.isPressed);
		}

		public void OnHeal(InputValue value)
		{
			Heal(value.isPressed);
		}

        public void OnGrenade(InputValue value)
        {
            GrenadeInput(value.isPressed);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}
        public void CrouchInput(bool newCrouchState)
        {
            crouch = newCrouchState;
        }

		public void Weapon1Input(bool newWeaponState)
		{
			weapon1 = newWeaponState;
			weapon2 = !newWeaponState;
			weapon3 = !newWeaponState;
		}
        public void Weapon2Input(bool newWeaponState)
        {
            weapon2 = newWeaponState;
			weapon1 = !newWeaponState;
			weapon3 = !newWeaponState;
        }
		public void Weapon3Input(bool newWeaponState)
        {
            weapon3 = newWeaponState;
            weapon1 = !newWeaponState;
			weapon2 = !newWeaponState;
        }
        public void RifleShoot(bool newRifleState)
		{
			rifleShoot = newRifleState;
		}

        public void GrenadeInput(bool newGrenadeInput)
        {
            grenade = newGrenadeInput;
        }

		public void Heal(bool newHealInput)
		{
			heal = newHealInput;
		}
        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}