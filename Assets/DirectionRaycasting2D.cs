using UnityEngine;

namespace Assets.Script.PhysicsHelpers
{
    public class DirectionRaycasting2D : MonoBehaviour
    {
        #region Public Fields

        public float rayDistance;
        public bool showRays;

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        /// Checks for collision at the botton.
        /// </summary>
        /// <returns>A <see cref="RaycastHit2D"/></returns>
        public RaycastHit2D CheckCollisionDown()
        {
            return ProcessRayCollision(Vector2.down);
        }

        /// <summary>
        /// Checks for collision at the left.
        /// </summary>
        /// <returns>A <see cref="RaycastHit2D"/></returns>
        public RaycastHit2D CheckCollisionLeft()
        {
            return ProcessRayCollision(Vector2.left);
        }

        /// <summary>
        /// Checks for collision at the right.
        /// </summary>
        /// <returns>A <see cref="RaycastHit2D"/></returns>
        public RaycastHit2D CheckCollisionRight()
        {
            return ProcessRayCollision(Vector2.right);
        }

        /// <summary>
        /// Checks for collision at the top.
        /// </summary>
        /// <returns>A <see cref="RaycastHit2D"/></returns>
        public RaycastHit2D CheckCollisionUp()
        {
            return ProcessRayCollision(Vector2.up);
        }

        #endregion Public Methods

        #region Private Methods

        void DrawRaycast()
        {
            if (showRays)
            {
                //draw up
                Debug.DrawLine(gameObject.transform.position, new Vector3(gameObject.transform.position.x,
                    gameObject.transform.position.y + rayDistance, gameObject.transform.position.z), Color.red);

                //draw down
                Debug.DrawLine(gameObject.transform.position, new Vector3(gameObject.transform.position.x,
                    gameObject.transform.position.y - rayDistance, gameObject.transform.position.z), Color.red);

                //draw left
                Debug.DrawLine(gameObject.transform.position, new Vector3(gameObject.transform.position.x - rayDistance,
                    gameObject.transform.position.y, gameObject.transform.position.z), Color.red);

                //draw right
                Debug.DrawLine(gameObject.transform.position, new Vector3(gameObject.transform.position.x + rayDistance,
                    gameObject.transform.position.y, gameObject.transform.position.z), Color.red);
            }
        }

        RaycastHit2D ProcessRayCollision(Vector2 direction)
        {
            var results = new RaycastHit2D[2];
            var result = new RaycastHit2D();
            if (Physics2D.RaycastNonAlloc(gameObject.transform.position, direction, results, rayDistance, 1 << 9) > 0)
            {
                result = results[0];
            }

            return result;
        }

        void Update()
        {
            DrawRaycast();
        }

        #endregion Private Methods
    }
}