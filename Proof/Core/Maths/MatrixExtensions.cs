using System.Numerics;

namespace Proof.Core.Maths
{
    internal static class Maths
    {
        public static float[] ToArray(this Matrix4x4 mat)
        {
            return new[]
            {
                mat.M11, mat.M12, mat.M13, mat.M14,
                mat.M21, mat.M22, mat.M23, mat.M24,
                mat.M31, mat.M32, mat.M33, mat.M34,
                mat.M41, mat.M42, mat.M43, mat.M44,
            };
        }
    }
}
