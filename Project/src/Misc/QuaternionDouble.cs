/*
 * This is a copy-pasted code snippet from the System.Numerics namespace in the .NET reference source.
 * The only change was to make the Quaternion operate on double values instead of float values.
 * This is a workaround for the lack of a Quaternion working on doubles or being templated in the System.Numerics namespace.
 */

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// https://github.com/microsoft/referencesource/blob/master/System.Numerics/System/Numerics/QuaternionDouble.cs#L342

using System.Globalization;

namespace System.Numerics
{
    /// <summary>
    /// A structure encapsulating a four-dimensional vector (x,y,z,w), 
    /// which is used to efficiently rotate an object about the (x,y,z) vector by the angle theta, where w = cos(theta/2).
    /// </summary>
    public struct QuaternionDouble : IEquatable<QuaternionDouble>
    {
        /// <summary>
        /// Specifies the X-value of the vector component of the QuaternionDouble.
        /// </summary>
        public double X;
        /// <summary>
        /// Specifies the Y-value of the vector component of the QuaternionDouble.
        /// </summary>
        public double Y;
        /// <summary>
        /// Specifies the Z-value of the vector component of the QuaternionDouble.
        /// </summary>
        public double Z;
        /// <summary>
        /// Specifies the rotation component of the QuaternionDouble.
        /// </summary>
        public double W;

        /// <summary>
        /// Returns a QuaternionDouble representing no rotation.
        /// </summary>
        public static QuaternionDouble Identity
        {
            get { return new QuaternionDouble(0, 0, 0, 1); }
        }

        /// <summary>
        /// Returns whether the QuaternionDouble is the identity Quaternion.
        /// </summary>
        public bool IsIdentity
        {
            get { return X == 0f && Y == 0f && Z == 0f && W == 1f; }
        }

        /// <summary>
        /// Constructs a QuaternionDouble from the given components.
        /// </summary>
        /// <param name="x">The X component of the QuaternionDouble.</param>
        /// <param name="y">The Y component of the QuaternionDouble.</param>
        /// <param name="z">The Z component of the QuaternionDouble.</param>
        /// <param name="w">The W component of the QuaternionDouble.</param>
        public QuaternionDouble(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Constructs a QuaternionDouble from the given vector and rotation parts.
        /// </summary>
        /// <param name="vectorPart">The vector part of the QuaternionDouble.</param>
        /// <param name="scalarPart">The rotation part of the QuaternionDouble.</param>
        public QuaternionDouble(Vector3 vectorPart, double scalarPart)
        {
            X = vectorPart.X;
            Y = vectorPart.Y;
            Z = vectorPart.Z;
            W = scalarPart;
        }

        /// <summary>
        /// Calculates the length of the QuaternionDouble.
        /// </summary>
        /// <returns>The computed length of the QuaternionDouble.</returns>
        public double Length()
        {
            double ls = X * X + Y * Y + Z * Z + W * W;

            return Math.Sqrt(ls);
        }

        /// <summary>
        /// Calculates the length squared of the QuaternionDouble. This operation is cheaper than Length().
        /// </summary>
        /// <returns>The length squared of the QuaternionDouble.</returns>
        public double LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>
        /// Divides each component of the QuaternionDouble by the length of the Quaternion.
        /// </summary>
        /// <param name="value">The source QuaternionDouble.</param>
        /// <returns>The normalized QuaternionDouble.</returns>
        public static QuaternionDouble Normalize(QuaternionDouble value)
        {
            QuaternionDouble ans;

            double ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;

            double invNorm = 1.0f / Math.Sqrt(ls);

            ans.X = value.X * invNorm;
            ans.Y = value.Y * invNorm;
            ans.Z = value.Z * invNorm;
            ans.W = value.W * invNorm;

            return ans;
        }

        /// <summary>
        /// Creates the conjugate of a specified QuaternionDouble.
        /// </summary>
        /// <param name="value">The QuaternionDouble of which to return the conjugate.</param>
        /// <returns>A new QuaternionDouble that is the conjugate of the specified one.</returns>
        public static QuaternionDouble Conjugate(QuaternionDouble value)
        {
            QuaternionDouble ans;

            ans.X = -value.X;
            ans.Y = -value.Y;
            ans.Z = -value.Z;
            ans.W = value.W;

            return ans;
        }

        /// <summary>
        /// Returns the inverse of a QuaternionDouble.
        /// </summary>
        /// <param name="value">The source QuaternionDouble.</param>
        /// <returns>The inverted QuaternionDouble.</returns>
        public static QuaternionDouble Inverse(QuaternionDouble value)
        {
            //  -1   (       a              -v       )
            // q   = ( -------------   ------------- )
            //       (  a^2 + |v|^2  ,  a^2 + |v|^2  )

            QuaternionDouble ans;

            double ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
            double invNorm = 1.0f / ls;

            ans.X = -value.X * invNorm;
            ans.Y = -value.Y * invNorm;
            ans.Z = -value.Z * invNorm;
            ans.W = value.W * invNorm;

            return ans;
        }

        /// <summary>
        /// Creates a QuaternionDouble from a vector and an angle to rotate about the vector.
        /// </summary>
        /// <param name="axis">The vector to rotate around.</param>
        /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
        /// <returns>The created QuaternionDouble.</returns>
        public static QuaternionDouble CreateFromAxisAngle(Vector3 axis, double angle)
        {
            QuaternionDouble ans;

            double halfAngle = angle * 0.5f;
            double s = Math.Sin(halfAngle);
            double c = Math.Cos(halfAngle);

            ans.X = axis.X * s;
            ans.Y = axis.Y * s;
            ans.Z = axis.Z * s;
            ans.W = c;

            return ans;
        }

        /// <summary>
        /// Creates a new QuaternionDouble from the given yaw, pitch, and roll, in radians.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the Y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the X-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the Z-axis.</param>
        /// <returns></returns>
        public static QuaternionDouble CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            //  Roll first, about axis the object is facing, then
            //  pitch upward, then yaw to face into the new heading
            double sr, cr, sp, cp, sy, cy;

            double halfRoll = roll * 0.5f;
            sr = Math.Sin(halfRoll);
            cr = Math.Cos(halfRoll);

            double halfPitch = pitch * 0.5f;
            sp = Math.Sin(halfPitch);
            cp = Math.Cos(halfPitch);

            double halfYaw = yaw * 0.5f;
            sy = Math.Sin(halfYaw);
            cy = Math.Cos(halfYaw);

            QuaternionDouble result;

            result.X = cy * sp * cr + sy * cp * sr;
            result.Y = sy * cp * cr - cy * sp * sr;
            result.Z = cy * cp * sr - sy * sp * cr;
            result.W = cy * cp * cr + sy * sp * sr;

            return result;
        }

        /// <summary>
        /// Creates a QuaternionDouble from the given rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The created QuaternionDouble.</returns>
        public static QuaternionDouble CreateFromRotationMatrix(Matrix4x4 matrix)
        {
            double trace = matrix.M11 + matrix.M22 + matrix.M33;

            QuaternionDouble q = new QuaternionDouble();

            if (trace > 0.0f)
            {
                double s = Math.Sqrt(trace + 1.0f);
                q.W = s * 0.5f;
                s = 0.5f / s;
                q.X = (matrix.M23 - matrix.M32) * s;
                q.Y = (matrix.M31 - matrix.M13) * s;
                q.Z = (matrix.M12 - matrix.M21) * s;
            }
            else
            {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
                {
                    double s = Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                    double invS = 0.5f / s;
                    q.X = 0.5f * s;
                    q.Y = (matrix.M12 + matrix.M21) * invS;
                    q.Z = (matrix.M13 + matrix.M31) * invS;
                    q.W = (matrix.M23 - matrix.M32) * invS;
                }
                else if (matrix.M22 > matrix.M33)
                {
                    double s = Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                    double invS = 0.5f / s;
                    q.X = (matrix.M21 + matrix.M12) * invS;
                    q.Y = 0.5f * s;
                    q.Z = (matrix.M32 + matrix.M23) * invS;
                    q.W = (matrix.M31 - matrix.M13) * invS;
                }
                else
                {
                    double s = Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                    double invS = 0.5f / s;
                    q.X = (matrix.M31 + matrix.M13) * invS;
                    q.Y = (matrix.M32 + matrix.M23) * invS;
                    q.Z = 0.5f * s;
                    q.W = (matrix.M12 - matrix.M21) * invS;
                }
            }

            return q;
        }

        /// <summary>
        /// Calculates the dot product of two QuaternionDoubles.
        /// </summary>
        /// <param name="quaternion1">The first source QuaternionDouble.</param>
        /// <param name="quaternion2">The second source QuaternionDouble.</param>
        /// <returns>The dot product of the QuaternionDoubles.</returns>
        public static double Dot(QuaternionDouble quaternion1, QuaternionDouble quaternion2)
        {
            return quaternion1.X * quaternion2.X +
                   quaternion1.Y * quaternion2.Y +
                   quaternion1.Z * quaternion2.Z +
                   quaternion1.W * quaternion2.W;
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="quaternion1">The first source QuaternionDouble.</param>
        /// <param name="quaternion2">The second source QuaternionDouble.</param>
        /// <param name="amount">The relative weight of the second source QuaternionDouble in the interpolation.</param>
        /// <returns>The interpolated QuaternionDouble.</returns>
        public static QuaternionDouble Slerp(QuaternionDouble quaternion1, QuaternionDouble quaternion2, double amount)
        {
            const double epsilon = 1e-6f;

            double t = amount;

            double cosOmega = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                             quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

            bool flip = false;

            if (cosOmega < 0.0f)
            {
                flip = true;
                cosOmega = -cosOmega;
            }

            double s1, s2;

            if (cosOmega > (1.0f - epsilon))
            {
                // Too close, do straight linear interpolation.
                s1 = 1.0f - t;
                s2 = (flip) ? -t : t;
            }
            else
            {
                double omega = Math.Acos(cosOmega);
                double invSinOmega = 1 / Math.Sin(omega);

                s1 = Math.Sin((1.0f - t) * omega) * invSinOmega;
                s2 = (flip)
                    ? -Math.Sin(t * omega) * invSinOmega
                    : Math.Sin(t * omega) * invSinOmega;
            }

            QuaternionDouble ans;

            ans.X = s1 * quaternion1.X + s2 * quaternion2.X;
            ans.Y = s1 * quaternion1.Y + s2 * quaternion2.Y;
            ans.Z = s1 * quaternion1.Z + s2 * quaternion2.Z;
            ans.W = s1 * quaternion1.W + s2 * quaternion2.W;

            return ans;
        }

        /// <summary>
        ///  Linearly interpolates between two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first source QuaternionDouble.</param>
        /// <param name="quaternion2">The second source QuaternionDouble.</param>
        /// <param name="amount">The relative weight of the second source QuaternionDouble in the interpolation.</param>
        /// <returns>The interpolated QuaternionDouble.</returns>
        public static QuaternionDouble Lerp(QuaternionDouble quaternion1, QuaternionDouble quaternion2, double amount)
        {
            double t = amount;
            double t1 = 1.0f - t;

            QuaternionDouble r = new QuaternionDouble();

            double dot = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                        quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

            if (dot >= 0.0f)
            {
                r.X = t1 * quaternion1.X + t * quaternion2.X;
                r.Y = t1 * quaternion1.Y + t * quaternion2.Y;
                r.Z = t1 * quaternion1.Z + t * quaternion2.Z;
                r.W = t1 * quaternion1.W + t * quaternion2.W;
            }
            else
            {
                r.X = t1 * quaternion1.X - t * quaternion2.X;
                r.Y = t1 * quaternion1.Y - t * quaternion2.Y;
                r.Z = t1 * quaternion1.Z - t * quaternion2.Z;
                r.W = t1 * quaternion1.W - t * quaternion2.W;
            }

            // Normalize it.
            double ls = r.X * r.X + r.Y * r.Y + r.Z * r.Z + r.W * r.W;
            double invNorm = 1.0f / Math.Sqrt(ls);

            r.X *= invNorm;
            r.Y *= invNorm;
            r.Z *= invNorm;
            r.W *= invNorm;

            return r;
        }

        /// <summary>
        /// Concatenates two QuaternionDoubles; the result represents the value1 rotation followed by the value2 rotation.
        /// </summary>
        /// <param name="value1">The first QuaternionDouble rotation in the series.</param>
        /// <param name="value2">The second QuaternionDouble rotation in the series.</param>
        /// <returns>A new QuaternionDouble representing the concatenation of the value1 rotation followed by the value2 rotation.</returns>
        public static QuaternionDouble Concatenate(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            // Concatenate rotation is actually q2 * q1 instead of q1 * q2.
            // So that's why value2 goes q1 and value1 goes q2.
            double q1x = value2.X;
            double q1y = value2.Y;
            double q1z = value2.Z;
            double q1w = value2.W;

            double q2x = value1.X;
            double q2y = value1.Y;
            double q2z = value1.Z;
            double q2w = value1.W;

            // cross(av, bv)
            double cx = q1y * q2z - q1z * q2y;
            double cy = q1z * q2x - q1x * q2z;
            double cz = q1x * q2y - q1y * q2x;

            double dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Flips the sign of each component of the quaternion.
        /// </summary>
        /// <param name="value">The source QuaternionDouble.</param>
        /// <returns>The negated QuaternionDouble.</returns>
        public static QuaternionDouble Negate(QuaternionDouble value)
        {
            QuaternionDouble ans;

            ans.X = -value.X;
            ans.Y = -value.Y;
            ans.Z = -value.Z;
            ans.W = -value.W;

            return ans;
        }

        /// <summary>
        /// Adds two QuaternionDoubles element-by-element.
        /// </summary>
        /// <param name="value1">The first source QuaternionDouble.</param>
        /// <param name="value2">The second source QuaternionDouble.</param>
        /// <returns>The result of adding the QuaternionDoubles.</returns>
        public static QuaternionDouble Add(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            ans.X = value1.X + value2.X;
            ans.Y = value1.Y + value2.Y;
            ans.Z = value1.Z + value2.Z;
            ans.W = value1.W + value2.W;

            return ans;
        }

        /// <summary>
        /// Subtracts one QuaternionDouble from another.
        /// </summary>
        /// <param name="value1">The first source QuaternionDouble.</param>
        /// <param name="value2">The second QuaternionDouble, to be subtracted from the first.</param>
        /// <returns>The result of the subtraction.</returns>
        public static QuaternionDouble Subtract(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            ans.X = value1.X - value2.X;
            ans.Y = value1.Y - value2.Y;
            ans.Z = value1.Z - value2.Z;
            ans.W = value1.W - value2.W;

            return ans;
        }

        /// <summary>
        /// Multiplies two QuaternionDoubles together.
        /// </summary>
        /// <param name="value1">The QuaternionDouble on the left side of the multiplication.</param>
        /// <param name="value2">The QuaternionDouble on the right side of the multiplication.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionDouble Multiply(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            double q1x = value1.X;
            double q1y = value1.Y;
            double q1z = value1.Z;
            double q1w = value1.W;

            double q2x = value2.X;
            double q2y = value2.Y;
            double q2z = value2.Z;
            double q2w = value2.W;

            // cross(av, bv)
            double cx = q1y * q2z - q1z * q2y;
            double cy = q1z * q2x - q1x * q2z;
            double cz = q1x * q2y - q1y * q2x;

            double dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Multiplies a QuaternionDouble by a scalar value.
        /// </summary>
        /// <param name="value1">The source QuaternionDouble.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionDouble Multiply(QuaternionDouble value1, double value2)
        {
            QuaternionDouble ans;

            ans.X = value1.X * value2;
            ans.Y = value1.Y * value2;
            ans.Z = value1.Z * value2;
            ans.W = value1.W * value2;

            return ans;
        }

        /// <summary>
        /// Divides a QuaternionDouble by another Quaternion.
        /// </summary>
        /// <param name="value1">The source QuaternionDouble.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static QuaternionDouble Divide(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            double q1x = value1.X;
            double q1y = value1.Y;
            double q1z = value1.Z;
            double q1w = value1.W;

            //-------------------------------------
            // Inverse part.
            double ls = value2.X * value2.X + value2.Y * value2.Y +
                       value2.Z * value2.Z + value2.W * value2.W;
            double invNorm = 1.0f / ls;

            double q2x = -value2.X * invNorm;
            double q2y = -value2.Y * invNorm;
            double q2z = -value2.Z * invNorm;
            double q2w = value2.W * invNorm;

            //-------------------------------------
            // Multiply part.

            // cross(av, bv)
            double cx = q1y * q2z - q1z * q2y;
            double cy = q1z * q2x - q1x * q2z;
            double cz = q1x * q2y - q1y * q2x;

            double dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Flips the sign of each component of the quaternion.
        /// </summary>
        /// <param name="value">The source QuaternionDouble.</param>
        /// <returns>The negated QuaternionDouble.</returns>
        public static QuaternionDouble operator -(QuaternionDouble value)
        {
            QuaternionDouble ans;

            ans.X = -value.X;
            ans.Y = -value.Y;
            ans.Z = -value.Z;
            ans.W = -value.W;

            return ans;
        }

        /// <summary>
        /// Adds two QuaternionDoubles element-by-element.
        /// </summary>
        /// <param name="value1">The first source QuaternionDouble.</param>
        /// <param name="value2">The second source QuaternionDouble.</param>
        /// <returns>The result of adding the QuaternionDoubles.</returns>
        public static QuaternionDouble operator +(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            ans.X = value1.X + value2.X;
            ans.Y = value1.Y + value2.Y;
            ans.Z = value1.Z + value2.Z;
            ans.W = value1.W + value2.W;

            return ans;
        }

        /// <summary>
        /// Subtracts one QuaternionDouble from another.
        /// </summary>
        /// <param name="value1">The first source QuaternionDouble.</param>
        /// <param name="value2">The second QuaternionDouble, to be subtracted from the first.</param>
        /// <returns>The result of the subtraction.</returns>
        public static QuaternionDouble operator -(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            ans.X = value1.X - value2.X;
            ans.Y = value1.Y - value2.Y;
            ans.Z = value1.Z - value2.Z;
            ans.W = value1.W - value2.W;

            return ans;
        }

        /// <summary>
        /// Multiplies two QuaternionDoubles together.
        /// </summary>
        /// <param name="value1">The QuaternionDouble on the left side of the multiplication.</param>
        /// <param name="value2">The QuaternionDouble on the right side of the multiplication.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionDouble operator *(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            double q1x = value1.X;
            double q1y = value1.Y;
            double q1z = value1.Z;
            double q1w = value1.W;

            double q2x = value2.X;
            double q2y = value2.Y;
            double q2z = value2.Z;
            double q2w = value2.W;

            // cross(av, bv)
            double cx = q1y * q2z - q1z * q2y;
            double cy = q1z * q2x - q1x * q2z;
            double cz = q1x * q2y - q1y * q2x;

            double dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Multiplies a QuaternionDouble by a scalar value.
        /// </summary>
        /// <param name="value1">The source QuaternionDouble.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the multiplication.</returns>
        public static QuaternionDouble operator *(QuaternionDouble value1, double value2)
        {
            QuaternionDouble ans;

            ans.X = value1.X * value2;
            ans.Y = value1.Y * value2;
            ans.Z = value1.Z * value2;
            ans.W = value1.W * value2;

            return ans;
        }

        /// <summary>
        /// Divides a QuaternionDouble by another Quaternion.
        /// </summary>
        /// <param name="value1">The source QuaternionDouble.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The result of the division.</returns>
        public static QuaternionDouble operator /(QuaternionDouble value1, QuaternionDouble value2)
        {
            QuaternionDouble ans;

            double q1x = value1.X;
            double q1y = value1.Y;
            double q1z = value1.Z;
            double q1w = value1.W;

            //-------------------------------------
            // Inverse part.
            double ls = value2.X * value2.X + value2.Y * value2.Y +
                       value2.Z * value2.Z + value2.W * value2.W;
            double invNorm = 1.0f / ls;

            double q2x = -value2.X * invNorm;
            double q2y = -value2.Y * invNorm;
            double q2z = -value2.Z * invNorm;
            double q2w = value2.W * invNorm;

            //-------------------------------------
            // Multiply part.

            // cross(av, bv)
            double cx = q1y * q2z - q1z * q2y;
            double cy = q1z * q2x - q1x * q2z;
            double cz = q1x * q2y - q1y * q2x;

            double dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given QuaternionDoubles are equal.
        /// </summary>
        /// <param name="value1">The first QuaternionDouble to compare.</param>
        /// <param name="value2">The second QuaternionDouble to compare.</param>
        /// <returns>True if the QuaternionDoubles are equal; False otherwise.</returns>
        public static bool operator ==(QuaternionDouble value1, QuaternionDouble value2)
        {
            return (value1.X == value2.X &&
                    value1.Y == value2.Y &&
                    value1.Z == value2.Z &&
                    value1.W == value2.W);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given QuaternionDoubles are not equal.
        /// </summary>
        /// <param name="value1">The first QuaternionDouble to compare.</param>
        /// <param name="value2">The second QuaternionDouble to compare.</param>
        /// <returns>True if the QuaternionDoubles are not equal; False if they are equal.</returns>
        public static bool operator !=(QuaternionDouble value1, QuaternionDouble value2)
        {
            return (value1.X != value2.X ||
                    value1.Y != value2.Y ||
                    value1.Z != value2.Z ||
                    value1.W != value2.W);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given QuaternionDouble is equal to this Quaternion instance.
        /// </summary>
        /// <param name="other">The QuaternionDouble to compare this instance to.</param>
        /// <returns>True if the other QuaternionDouble is equal to this instance; False otherwise.</returns>
        public bool Equals(QuaternionDouble other)
        {
            return (X == other.X &&
                    Y == other.Y &&
                    Z == other.Z &&
                    W == other.W);
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this QuaternionDouble instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this QuaternionDouble; False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is QuaternionDouble)
            {
                return Equals((QuaternionDouble)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns a String representing this QuaternionDouble instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            CultureInfo ci = CultureInfo.CurrentCulture;

            return String.Format(ci, "{{X:{0} Y:{1} Z:{2} W:{3}}}", X.ToString(ci), Y.ToString(ci), Z.ToString(ci), W.ToString(ci));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }
    }
}