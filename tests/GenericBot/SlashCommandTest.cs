using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Enums;

namespace GenericBot
{
    [SlashCommand("te", InteractionType.SubCommand, "teste comand", GuildId = "1336392659992051762")]
    public class SlashCommandTest : CommandBase, ISlashCommand
    {
        public async Task<bool> RunAsync(InteractionContext context, SlashCommandParamsResult paramsResult)
        {
            if (paramsResult.Options.Any())
            {
                var optionOne = paramsResult.Options.FirstOrDefault(x => x.Name == "param1");
                if (optionOne != null)
                {
                    if (optionOne.Value == "animal_dog")
                    {
                        Console.WriteLine("Funcionou!");
                    }
                }
            }

            return true;
        }

        public List<SlashCommandOptions> BuildOptions()
        {
            return new List<SlashCommandOptions>()
            {
                new()
                {
                    Name = "param1",
                    Description = "Primeiro parâmetro",
                    Type = InteractionType.String,
                    Required = true,
                    Choices = new List<SlashCommandChoices>()
                    {
                        new ()
                        {
                            Name = "Dog",
                            Value = "animal_dog"
                        }
                    }
                }
            };
        }
    }
}
