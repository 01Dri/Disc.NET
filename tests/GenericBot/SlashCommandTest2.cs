//using Disc.NET.Commands;
//using Disc.NET.Commands.Attributes;
//using Disc.NET.Commands.Contexts;
//using Disc.NET.Shared.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GenericBot
//{
//    [SlashCommand("teste2", InteractionType.String, "teste comand2")]

//    internal class SlashCommandTest2 : CommandBase, ISlashCommand<CommandContext>
//    {
//        public Task<bool> RunAsync(CommandContext context, SlashCommandParamsResult result)
//        {
//            throw new NotImplementedException();
//        }

//        public List<SlashCommandOptions> BuildOptions()
//        {
//            return new List<SlashCommandOptions>()
//            {
//                new()
//                {
//                    Name = "param1",
//                    Description = "Primeiro parâmetro",
//                    Type = InteractionType.String,
//                    Required = true,
//                    Choices = new List<SlashCommandChoices>()
//                    {
//                        new ()
//                        {
//                            Name = "Dog",
//                            Value = "animal_dog"
//                        }
//                    }
//                }
//            };
//        }
//    }
//}
