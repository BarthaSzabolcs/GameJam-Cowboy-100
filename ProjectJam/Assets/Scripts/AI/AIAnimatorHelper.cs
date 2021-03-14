using System;

using UnityEngine;

namespace GameJam.AI
{
    [Serializable]
    public class AIAnimatorHelper
    {
        #region Datamembers

        #region Enums

        private enum AnimationClips 
        { 
            Angry_Down, 
            Angry_Up, 
            Angry_Left, 
            Angry_Right,

            Calm_Down,
            Calm_Up,
            Calm_Left,
            Calm_Right
        };

        #endregion
        #region Editor Settings

        [SerializeField] private Animator animator;

        #endregion
        #region Public Properties

        public bool Calm { get; set; }

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
            var animationOffSet = Calm ? 4 : 0;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    CurrentAnimation = AnimationClips.Angry_Right + animationOffSet;
                }
                else
                {
                    CurrentAnimation = AnimationClips.Angry_Left + animationOffSet;
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    CurrentAnimation = AnimationClips.Angry_Up + animationOffSet;
                }
                else
                {
                    CurrentAnimation = AnimationClips.Angry_Down + animationOffSet;
                }
            }
        }


    }
}