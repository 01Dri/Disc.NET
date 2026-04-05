using Disc.NET.Client.SDK.Enums;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Client.SDK.Messages.Components.Enums;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.MessageBuilders;
using Disc.NET.Shared.Constraints;
using Disc.NET.Shared.Exceptions;

namespace Disc.NET.Commands
{
    public class Message : ApiMessage
    {
        
        public List<MessageFlag>? MessageFlags { get; set; }

        public List<IActionRowBuilder> ActionRows { get; set; } = [];
        
        public ApiMessage Build()
        {
            Flags = MessageFlags?.Aggregate(0L, (current, flag) => current | (long)flag) ?? 0L;
            Components = MountActionRows();
            Type ??= 4;
            return this;
        }


        private List<object> MountActionRows()
        {
            if (ActionRows.Count > 5)
                throw new DiscNetGenericException("The message cannot contain more than 5 top-level components.");

            var results = new List<object>();

            // Normalize for Select Menus (Max 1 per action row)
            NormalizeActionRows(ActionRowConstraint.MAX_SELECT_MENUS_PER_ACTION_ROW, components => 
                components.Any(c => c.Type == MessageComponentType.SelectMenu),
                message => new ActionRowBuilder().AddComponent<ContextBase>(message.First()));

            // Normalize for Buttons (Max 5 per action row)
            NormalizeActionRows(ActionRowConstraint.MAX_BUTTONS_PER_ACTION_ROW, components => 
                components.All(c => c.Type == MessageComponentType.Button),
                message => 
                {
                    var builder = new ActionRowBuilder();
                    message.ForEach(c => builder.AddComponent<ContextBase>(c));
                    return builder;
                });

            ActionRows.ForEach(x => results.Add(x.Build()));
            return results;
        }

        private void NormalizeActionRows(int quantityComponentPerActionRow,
            Func<List<IMessageComponent>, bool> predicate,
            Func<List<IMessageComponent>, IActionRowBuilder> createBuilderFunc)
        {
            var actionRowsToProcess = ActionRows.Where(x => predicate(x.Components)).ToList();

            if (actionRowsToProcess.Count == 0) return;
            
            int actionRowsPerMessage = ActionRowConstraint.MAX_ACTION_ROWS_PER_MESSAGE;
            
            int availableActionRowSlots = actionRowsPerMessage - ActionRows.Count;
            var invalidActionRows = actionRowsToProcess.Where(x => x.Components.Count > quantityComponentPerActionRow).ToList();
            var containsActionRowsInvalids = invalidActionRows.Count > 0;

            if (!containsActionRowsInvalids) return;
            
            if (availableActionRowSlots <= 0)
            {
                throw new DiscNetGenericException($"The message cannot contain more than {actionRowsPerMessage} top-level components.");
            }

            int numberNecessaryToCreateNewActionRows;
            if (quantityComponentPerActionRow > 1)
            {
                numberNecessaryToCreateNewActionRows = invalidActionRows
                    .Sum(x => (int)Math.Ceiling((double)x.Components.Count / quantityComponentPerActionRow) - 1);
            }
            else
            {
                numberNecessaryToCreateNewActionRows = invalidActionRows
                    .Sum(x => x.Components.Count - 1);
            }

            if (numberNecessaryToCreateNewActionRows > availableActionRowSlots)
            {
                throw new DiscNetGenericException(
                    $"The message cannot contain more than {actionRowsPerMessage} top-level components and there is no space to split components into new Action Rows.");
            }

            int newActionRowsCount = 0;
            foreach (var actionRow in invalidActionRows)
            {
                var messageComponents = actionRow.Components.ToList();

                int index = 0;
                while (messageComponents.Count - index > quantityComponentPerActionRow &&
                       newActionRowsCount < availableActionRowSlots)
                {
                    IActionRowBuilder? newActionRow;

                    if (quantityComponentPerActionRow > 1)
                    {
                        var componentsToMove = messageComponents
                            .Skip(index + quantityComponentPerActionRow) // Keep the first N in current row, move others
                            .Take(quantityComponentPerActionRow)
                            .ToList();
                        
                        componentsToMove.ForEach(x => actionRow.Components.Remove(x));
                        index += quantityComponentPerActionRow;
                        newActionRow = createBuilderFunc.Invoke(componentsToMove);
                    }
                    else
                    {
                        // For Select Menus (max 1), move everything after the first one
                        var componentsToMove = messageComponents.Skip(1).ToList();
                        componentsToMove.ForEach(x => actionRow.Components.Remove(x));
                        
                        // We need to create a new action row for each extra select menu
                        foreach(var comp in componentsToMove)
                        {
                             if (newActionRowsCount < availableActionRowSlots)
                             {
                                 ActionRows.Add(createBuilderFunc.Invoke([comp]));
                                 newActionRowsCount++;
                             }
                             else
                             {
                                 throw new DiscNetGenericException("Not enough slots for all select menus.");
                             }
                        }
                        break; // Already handled all components for this row
                    }

                    if (newActionRow != null)
                    {
                        ActionRows.Add(newActionRow);
                        newActionRowsCount++;
                    }
                }
            }
        }
    }
}
