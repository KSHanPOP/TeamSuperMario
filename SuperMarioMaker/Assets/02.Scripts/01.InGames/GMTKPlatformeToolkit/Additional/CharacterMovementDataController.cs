using UnityEngine;

namespace GMTK.PlatformerToolkit {
    
    public class CharacterMovementDataController : MonoBehaviour {
        [SerializeField] PresetObject _preset;

        [SerializeField]
        characterMovement _moveScript;
        [SerializeField]
        characterJump _jumpScript;

        void Awake() {

            SetMoveData();
            SetJumpData();
        }

        private void SetMoveData()
        {
            if (_moveScript == null)
                return;

            _moveScript.maxAcceleration = _preset.Acceleration;
            _moveScript.maxSpeed = _preset.TopSpeed;
            _moveScript.maxDecceleration = _preset.Deceleration;
            _moveScript.maxTurnSpeed = _preset.TurnSpeed;
        }
        private void SetJumpData()
        {
            if ( _jumpScript == null) 
                return;

            _moveScript.maxAirAcceleration = _preset.AirControl;
            _moveScript.maxAirDeceleration = _preset.AirBrake;
            _jumpScript.jumpHeight = _preset.JumpHeight;
            _jumpScript.timeToJumpApex = _preset.TimeToApex;
            _jumpScript.downwardMovementMultiplier = _preset.DownwardMovementMultiplier;
            _jumpScript.jumpCutOff = _preset.JumpCutoff;
            _jumpScript.maxAirJumps = _preset.DoubleJump;
            _jumpScript.variablejumpHeight = _preset.VariableJumpHeight;
            _moveScript.maxAirTurnSpeed = _preset.AirControlActual;
        }
    }
}