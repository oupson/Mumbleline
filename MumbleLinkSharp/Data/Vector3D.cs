namespace MumbleLinkSharp.Data
{
    /// <summary>
    /// Represent an array of 3 floats.
    /// </summary>
    public class Vector3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

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
