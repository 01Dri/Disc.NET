namespace Disc.NET.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PrefixCommandAttribute : Attribute
    {
        public string Name { get; init; }

        public PrefixCommandAttribute(string name)
        {
            Name = name;
        }
    }
}