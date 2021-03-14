using System;

using UnityEngine;

namespace GameJam.AI
{
    [Serializable]
    public class PlayerAnimationHelper
    {
        #region Datamembers

        #region Enums

        private enum AnimationClips 
        { 
            Down,
            Up, 
            Left, 
            Right,
        };

        #endregion
        #region Editor Settings

        [SerializeField] private Animator animator;

        #endregion
        #region Private Properties

        private AnimationClips CurrentAnimation
        {
            set
            {
                if (_currentAnimation != value)
                {
                    _currentAnimation = value;
                    animator.Play(value.ToString());
                }
            }
            get
            {
                return _currentAnimation;
            }
        }

        #endregion
        #region Backing Fields

        private AnimationClips _currentAnimation;

        #endregion

        #endregion

        public void RefreshDirection(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    CurrentAnimation = AnimationClips.Right;
                }
                else
                {
                    CurrentAnimation = AnimationClips.Left;
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    CurrentAnimation = AnimationClips.Up;
                }
                else
                {
                    CurrentAnimation = AnimationClips.Down;
                }
            }
        }


    }
}