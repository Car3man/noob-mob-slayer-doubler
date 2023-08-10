using UnityEngine;

namespace Utility
{
    public class MathHelper
    {
        /// <summary>
        /// Rotates a vector using the Rodriguez rotation formula
        /// about an arbitrary axis.
        /// </summary>
        /// <param name="vector">The vector to be rotated.</param>
        /// <param name="axis">The rotation axis.</param>
        /// <param name="angle">The rotation angle.</param>
        /// <returns>The rotated vector</returns>
        public static Vector3 RotateVector(Vector3 vector, Vector3 axis, float angle)
        {
            Vector3 vxp = Vector3.Cross(axis, vector);
            Vector3 vxvxp = Vector3.Cross(axis, vxp);
            return vector + Mathf.Sin(angle) * vxp + (1 - Mathf.Cos(angle)) * vxvxp;
        }

        /// <summary>
        /// Rotates a vector about a point in space.
        /// </summary>
        /// <param name="vector">The vector to be rotated.</param>
        /// <param name="pivot">The pivot point.</param>
        /// <param name="axis">The rotation axis.</param>
        /// <param name="angle">The rotation angle.</param>
        /// <returns>The rotated vector</returns>
        public static Vector3 RotateVectorAboutPoint(Vector3 vector, Vector3 pivot, Vector3 axis, float angle)
        {
            return pivot+ RotateVector(vector - pivot, axis, angle);   
        }
    }
}