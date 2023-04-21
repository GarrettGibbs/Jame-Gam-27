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
            if (_playerFacing == PlayerFacing.up) _animator.Play(_idleUpAnim);
            if (_playerFacing == PlayerFacing.down) _animator.Play(_idleDownAnim);
            if (_playerFacing == PlayerFacing.side) _animator.Play(_idleSideAnim);
        }
        else
        {
            if(input.y > .01)
            {
                _playerFacing = PlayerFacing.up;
                _animator.Play(_walkUpAnim);
            }
            else if(input.y < -.01)
            {
                _playerFacing = PlayerFacing.down;
                _animator.Play(_walkDownAnim);
            }
            else if (input.x > .01)
            {
                _playerFacing = PlayerFacing.side;
                _sprite.flipX = true;
                _animator.Play(_walkSideAnim);
            }
            else if (input.x < -.01)
            {
                _playerFacing = PlayerFacing.side;
                _sprite.flipX = false;
                _animator.Play(_walkSideAnim);
            }
        }
    }


}
