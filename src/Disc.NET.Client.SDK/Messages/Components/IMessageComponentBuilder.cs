namespace Disc.NET.Client.SDK.Messages.Components
{
    public interface IMessageComponentBuilder
    {
		public string? Id { get; }
        public List<object> Components { get; }
        object Build();
    }
}
