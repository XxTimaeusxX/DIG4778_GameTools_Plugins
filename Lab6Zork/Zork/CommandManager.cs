using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork
{
    public class CommandManager
    {
        public CommandManager() => mCommands = new HashSet<Command>();
        public CommandManager(IEnumerable<Command> commands) => mCommands = new HashSet<Command>(commands);

        public CommandContext Parse(string commandString)
        {
            var commandQuery = from command in mCommands
                               where command.Verbs.Contains(commandString, StringComparer.OrdinalIgnoreCase)
                               select new CommandContext(commandString, command);
            return commandQuery.FirstOrDefault();
        }
        public bool PerformedCommand(Game game, string  commandString)
        {
            bool result;
            CommandContext commandContext = Parse(commandString);
            if(commandContext.Command !=null)
            {
                commandContext.Command.Action(game, commandContext);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public void AddCommand(Command command) => mCommands.Add(command);
        public void RemoveCommand(Command command) => mCommands.Remove(command);
        public void Addcommands(IEnumerable<Command> commands) => mCommands.UnionWith(commands);
        public void ClearCommands() => mCommands.Clear();

        public HashSet<Command> mCommands;
            
    }
}
