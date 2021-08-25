namespace Mumbleline.MumbleLink.Data
{
    public class Vector3D
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3D() : this(0, 0, 0) { }
        public Vector3D(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public override string ToString()
        {
            return string.Format("Position(X : {0}, Y : {1}, Z: {2})", this.X, this.Y, this.Z);
        }
    }
}
