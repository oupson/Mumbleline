using System.Text;

namespace MumbleLinkSharp.Data
{
    /// <summary>
    /// An abstraction over mumble's linked mem.
    /// You should always use this class instead of linux or windows linked mem
    /// </summary>
    public class LinkInformations
    {
        public uint UiVersion { get; set; }
        public uint UiTick { get; set; }
        public Vector3D AvatarPosition { get; set; }
        public Vector3D AvatarFront { get; set; }
        public Vector3D AvatarTop { get; set; }
        public string Name { get; set; }
        public Vector3D CameraPosition { get; set; }
        public Vector3D CameraFront { get; set; }
        public Vector3D CameraTop { get; set; }
        public string Identity { get; set; }
        public uint ContextLen { get; set; }
        public string Context { get; set; }
        public string Description { get; set; }

        public LinkInformations()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name);
            sb.Append("(");
            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
            {
                sb.Append(property.Name);
                sb.Append(" : ");
                if (property.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    sb.Append(property.GetValue(this, null));
                }

                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");

            return sb.ToString();
        }
    }
}