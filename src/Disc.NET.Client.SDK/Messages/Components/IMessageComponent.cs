namespace Disc.NET.Client.SDK.Messages.Components
{
	public interface IMessageComponent
    {

            //⚠️ Outro problema(importante)
            //Você está usando:

            //CustomId = "test_button"
            //👉 Isso vai dar problema real quando:

            //Dois usuários clicarem

            //Ou você tiver múltiplas mensagens

            //💥 Porque o callback vai sobrescrever no dictionary

            //✅ Solução recomendada(muito importante)
            //Use CustomId único:

            //CustomId = $"test_button:{Guid.NewGuid()}"

		string? CustomId { get; set; }
    }
}
