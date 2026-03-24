using Disc.NET.Client.SDK.Enums;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Client.SDK.Messages.Components;
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

            NormalizeActionRows<ActionRowSelectMenuBuilder>(ActionRowConstraint.MAX_SELECT_MENUS_PER_ACTION_ROW, message =>
                new ActionRowSelectMenuBuilder().AddComponent<ContextBase>(message.First()));

            ActionRows.ForEach(x => results.Add(x.Build()));
            return results;
        }

        /// <summary>
        /// Normalizes action rows of a specific component type to comply with Discord message constraints.
        /// 
        /// This method validates the maximum number of action rows per message and the maximum number
        /// of components allowed per action row. If an action row exceeds the allowed number of components,
        /// it is automatically split into multiple valid action rows.
        /// </summary>
        /// <typeparam name="T">
        /// The type of action row builder to process (e.g., select menu rows or button rows).
        /// </typeparam>
        /// <param name="quantityComponentPerActionRow">
        /// The maximum number of components allowed per action row for the given component type.
        /// </param>
        /// <param name="createBuilderFunc">
        /// A factory function responsible for creating a new action row builder from a list
        /// of message components.
        /// </param>
        /// <exception cref="DiscNetClientSdkException">
        /// Thrown when the message exceeds Discord constraints, such as:
        /// - Maximum number of action rows per message
        /// - Insufficient available action row slots to split invalid rows
        /// </exception>
        /// <remarks>
        /// This method mutates the <see cref="ActionRow"/> collection by:
        /// - Removing excess components from invalid action rows
        /// - Adding newly created action rows to the message
        /// </remarks>

        private void NormalizeActionRows<T>(int quantityComponentPerActionRow,
            Func<List<IMessageComponent>, IActionRowBuilder> createBuilderFunc)
            where T : IActionRowBuilder
        {
            var actionRows = ActionRows.Where(x => x is T).ToList();

            if (actionRows.Count == 0) return;
            int actionRowsPerMessage = ActionRowConstraint.MAX_ACTION_ROWS_PER_MESSAGE;
            int actionRowsCount = actionRows.Count;
            if (actionRowsCount > actionRowsPerMessage)
            {
                throw new DiscNetGenericException(
                    $"The message cannot contain more than {actionRowsPerMessage} top-level components.");
            }

            int availableActionRowSlots = actionRowsPerMessage - ActionRows.Count;
            var invalidActionRows = actionRows.Where(x => x.Components.Count > quantityComponentPerActionRow).ToList();
            var containsActionRowsInvalids = invalidActionRows.Count > 0;

            if (!containsActionRowsInvalids) return;
            if (availableActionRowSlots == 0 && containsActionRowsInvalids)
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
                    $"The message cannot contain more than {actionRowsPerMessage} top-level components.");
            }

            int newActionRowsCount = 0;
            foreach (var actionRow in invalidActionRows)
            {
                var messageComponents = actionRow.Components
                    .OfType<IMessageComponent>()
                    .ToList();

                int index = 0;
                while (messageComponents.Count - index > quantityComponentPerActionRow &&
                       newActionRowsCount < availableActionRowSlots)
                {
                    IActionRowBuilder? newActionRow;

                    if (quantityComponentPerActionRow > 1)
                    {
                        var components = messageComponents
                            .Skip(index)
                            .Take(quantityComponentPerActionRow)
                            .ToList();
                        components.ForEach(x => actionRow.Components.Remove(x));
                        index += quantityComponentPerActionRow;
                        newActionRow = createBuilderFunc.Invoke(components);
                    }
                    else
                    {
                        var component = messageComponents[index];

                        actionRow.Components.Remove(component);
                        index++;
                        newActionRow = createBuilderFunc.Invoke([component]);
                    }

                    ActionRows.Add(newActionRow);
                    newActionRowsCount++;
                }
            }
        }

    }
}
