using UnityEngine;

public enum PlayerFacing { up, down, side }

public class PlayerAnimator : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Animator _animator;

    private string _walkUpAnim = "Walk_Up";
    private string _walkDownAnim = "Walk_Down";
    private string _walkSideAnim = "Walk_Side";
    private string _idleDownAnim = "Idle_Down";
    private string _idleUpAnim = "Idle_Up";
    private string _idleSideAnim = "Idle_Side";
    private PlayerFacing _playerFacing;

    private string currentAnimation;


    void Awake()
    {
        _playerFacing = PlayerFacing.down;
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetAnimation(Vector2 input)
    {
        if(input.magnitude == 0)
        {
            if (_playerFacing == PlayerFacing.up) _animator.Play(_idleUpAnim); currentAnimation = _idleUpAnim;
            if (_playerFacing == PlayerFacing.down) _animator.Play(_idleDownAnim); currentAnimation = _idleDownAnim;
            if (_playerFacing == PlayerFacing.side) _animator.Play(_idleSideAnim); currentAnimation = _idleSideAnim;
        }
        else
        {
            if(input.y > .01)
            {
                _playerFacing = PlayerFacing.up;
                if (currentAnimation == _walkUpAnim) return;
                _animator.Play(_walkUpAnim);
                currentAnimation = _walkUpAnim;
            }
            else if(input.y < -.01)
            {
                _playerFacing = PlayerFacing.down;
                if (currentAnimation == _walkDownAnim) return;
                _animator.Play(_walkDownAnim);
                currentAnimation = _walkDownAnim;
            }
            else if (input.x > .01)
            {
                _playerFacing = PlayerFacing.side;
                _sprite.flipX = true;
                if(currentAnimation == _walkSideAnim) return;
                _animator.Play(_walkSideAnim);
                currentAnimation = _walkSideAnim;
            }
            else if (input.x < -.01)
            {
                _playerFacing = PlayerFacing.side;
                _sprite.flipX = false;
                if (currentAnimation == _walkSideAnim) return;
                _animator.Play(_walkSideAnim);
                currentAnimation = _walkSideAnim;
            }
        }
    }

    public void TurnTowards(Vector2 input) {
        if (input.y > .01) {
            _playerFacing = PlayerFacing.up;
        } else if (input.y < -.01) {
            _playerFacing = PlayerFacing.down;
        } else if (input.x > .01) {
            _playerFacing = PlayerFacing.side;
            _sprite.flipX = true;
        } else if (input.x < -.01) {
            _playerFacing = PlayerFacing.side;
            _sprite.flipX = false;
            if (currentAnimation == _walkSideAnim) return;
        }
        if (_playerFacing == PlayerFacing.up) _animator.Play(_idleUpAnim); currentAnimation = _idleUpAnim;
        if (_playerFacing == PlayerFacing.down) _animator.Play(_idleDownAnim); currentAnimation = _idleDownAnim;
        if (_playerFacing == PlayerFacing.side) _animator.Play(_idleSideAnim); currentAnimation = _idleSideAnim;
    }


}
