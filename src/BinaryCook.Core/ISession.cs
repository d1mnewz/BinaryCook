using BinaryCook.Core.Commands;

namespace BinaryCook.Core
{
    public interface ISession
    {
        ICorrelation Correlation { get; }
        IUser User { get; }
        IClock Clock { get; }
    }
}