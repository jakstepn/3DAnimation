using GrafikaProjekt4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKtoolkit
{
    public class Matrices
    {
        public static CustomMatrix M(double width, double height, Translation translation, double angle)
        {
            CustomMatrix p = P(width, height),
                t = T(translation),
                ry = Ry(angle);
            return p * t * ry;
        }

        public static CustomMatrix P(double width, double height)
        {
            double[,] tab = new double[4, 4]
            {
                { height / width, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 1 },
                { 0, 0, -1, 0 }
            };
            CustomMatrix res = new CustomMatrix(tab, new MatrixSize(4, 4));
            return res;
        }

        /// <summary>
        /// Scale matrix
        /// </summary>
        /// <returns></returns>
        public static CustomMatrix S(Scale scale)
        {
            double[,] tab = new double[4, 4]
            {
                { scale.xAxis, 0, 0, 0 },
                { 0, scale.yAxis, 0, 0 },
                { 0, 0, scale.zAxis, 0 },
                { 0, 0, 0, 1 }
            };
            CustomMatrix res = new CustomMatrix(tab, new MatrixSize(4, 4));
            return res;
        }

        /// <summary>
        /// Translation matrix
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="distanceFromCamera"></param>
        /// <returns></returns>
        public static CustomMatrix T(Translation translation)
        {
            double[,] tab = new double[4, 4]
            {
                { 1, 0, 0, translation.xAxis },
                { 0, 1, 0, translation.yAxis },
                { 0, 0, 1, translation.zAxis },
                { 0, 0, 0, 1 }
            };
            CustomMatrix res = new CustomMatrix(tab, new MatrixSize(4, 4));
            return res;
        }

        public static CustomMatrix T(Position translateToPostion)
        {
            return T(new Translation(
                translateToPostion.X,
                translateToPostion.Y,
                translateToPostion.Z
                ));
        }

        /// <summary>
        /// Rotate y matrix
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static CustomMatrix Ry(double angle)
        {
            double[,] tab = new double[4, 4]
            {
                { Math.Cos(angle), 0, -Math.Sin(angle), 0 },
                { 0, 1, 0, 0 },
                { Math.Sin(angle), 0, Math.Cos(angle), 0 },
                { 0, 0, 0, 1 }
            };
            CustomMatrix res = new CustomMatrix(tab, new MatrixSize(4, 4));
            return res;
        }

        /// <summary>
        /// Rotate x matrix
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static CustomMatrix Rx(double angle)
        {
            double[,] tab = new double[4, 4]
            {
                { 1, 0, 0, 0 },
                { 0, Math.Cos(angle), -Math.Sin(angle), 0 },
                { 0, Math.Sin(angle), Math.Cos(angle), 0 },
                { 0, 0, 0, 1 }
            };
            CustomMatrix res = new CustomMatrix(tab, new MatrixSize(4, 4));
            return res;
        }

        /// <summary>
        /// Rotate z matrix
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static CustomMatrix Rz(double angle)
        {
            double[,] tab = new double[4, 4]
            {
                { Math.Cos(angle), -Math.Sin(angle), 0, 0 },
                { Math.Sin(angle), Math.Cos(angle), 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };
            CustomMatrix res = new CustomMatrix(tab, new MatrixSize(4, 4));
            return res;
        }

        /// <summary>
        /// Projection [-1, 1] matrix. Maps Z coordinate after projecting it on a value from [-1, 1]. 
        /// </summary>
        /// <param name="n">distance to a closer face</param>
        /// <param name="f">distance to a further face</param>
        /// <param name="fov">camera view angle in radians</param>
        /// <param name="aspect">acpect ratio of the screen</param>
        /// <returns></returns>
        public static CustomMatrix Proj(double n, double f, double fov, double aspect)
        {

            double val11 = 1.0 / (Math.Tan(fov / 2.0) * aspect),
                val22 = 1 / Math.Tan(fov / 2.0),
                val33 = (f + n) / (f - n),
                val34 = -2.0 * f * n / (f - n);

            double[,] tab = new double[4, 4]
            {
                { val11, 0, 0, 0 },
                { 0, val22, 0, 0 },
                { 0, 0, val33, val34 },
                { 0, 0, 1, 0 }
            };
            CustomMatrix res = new CustomMatrix(tab, new MatrixSize(4, 4));
            return res;
        }

        /// <summary>
        /// Camera view matrix
        /// </summary>
        /// <param name="D">Vector 3x1 - distance from target point T to camera position P</param>
        /// <param name="P">Vector 3x1 - camera position</param>
        /// <param name="uworld">Up vector</param>
        /// <returns></returns>
        public static CustomMatrix View(ICamera camera)
        {
            CustomVector Uworld = camera.upVector;
            CustomVector R = (Uworld * camera.toCamVector).Normalized();
            CustomVector U = (camera.toCamVector * R).Normalized();

            double[,] mat1 = new double[4, 4]
            {
                { R.Get(0, 0), R.Get(1, 0), R.Get(2, 0), 0 },
                { U.Get(0,0), U.Get(1, 0), U.Get(2,0), 0 },
                { camera.toCamVector.Get(0,0), camera.toCamVector.Get(1,0),
                    camera.toCamVector.Get(2,0), 0 },
                { 0, 0, 0, 1 }
            };
            double[,] mat2 = new double[4, 4]
            {
                { 1, 0, 0, -camera.position.Get(0, 0) },
                { 0, 1, 0, -camera.position.Get(1,0) },
                { 0, 0, 1, -camera.position.Get(2,0) },
                { 0, 0, 0, 1 }
            };

            CustomMatrix cmat1 = new CustomMatrix(mat1, new MatrixSize(4, 4));
            CustomMatrix cmat2 = new CustomMatrix(mat2, new MatrixSize(4, 4));

            return cmat1 * cmat2;
        }

        public static CustomMatrix Identity()
        {
            double[,] mat = new double[4, 4]
            {
                { 1,0,0,0 },
                { 0,1,0,0 },
                { 0,0,1,0 },
                { 0,0,0,1 },
            };
            return new CustomMatrix(mat, new MatrixSize(4, 4));
        }

        /// <summary>
        /// Translate to a centerPoint defined in transformation and then
        /// apply rotations, scale, and translation relative to a new center
        /// point.
        /// </summary>
        /// <param name="distanceToACameraFromPoint"></param>
        /// <param name="cameraPosition"></param>
        /// <param name="transformation">contains data required for:
        /// translation, rotation, scale and a center point</param>
        /// <returns></returns>
        /// 
        public static CustomMatrix PVM(ICamera camera,
            MotionTransformation transformation)
        {
            return Proj(ProjectionValues.distanceToACloserFace,
                ProjectionValues.distanceToAFurtherFace,
                ProjectionValues.fieldOfView,
                ProjectionValues.aspectRatio) *
                View(camera) * Transformation(transformation);
        }
        public static CustomMatrix PV(ICamera camera)
        {
            return Proj(ProjectionValues.distanceToACloserFace,
                ProjectionValues.distanceToAFurtherFace,
                ProjectionValues.fieldOfView,
                ProjectionValues.aspectRatio) *
                View(camera);
        }
        public static CustomMatrix Transformation(MotionTransformation transformation)
        {
            return T(transformation.centerPoint) *
                    Rx(transformation.angle.xAngle) *
                    Ry(transformation.angle.yAngle) *
                    Rz(transformation.angle.zAngle) *
                    T(transformation.translation) *
                    S(transformation.scale);
        }
    }

}
