namespace BinaryCook.Core.Commands
{
    public interface ICommand
    {
        Correlation Correlation { get; }
    }

    public abstract class Command : ICommand
    {
        public Correlation Correlation { get; } = new Correlation();

        protected Command()
        {
        }
    }

    public class NullCommand : Command
    {
    }
}