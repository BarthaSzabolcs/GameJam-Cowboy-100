using UnityEngine;

namespace GameJam
{
    public class SpriteHelper : MonoBehaviour
    {
        #region Datamembers

        #region Private Fields

        private Transform cameraTransform;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            var horizontalScale = new Vector3(0, 1, 1);

            var cameraPosition = cameraTransform.position;
            cameraPosition.Scale(horizontalScale);

            var position = transform.position;
            position.Scale(horizontalScale);

            transform.forward = (cameraPosition - position).normalized;
        }
        
        #endregion
        
        #endregion
    }
}
