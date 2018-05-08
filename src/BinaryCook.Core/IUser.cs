using BinaryCook.Core.Code;

namespace BinaryCook.Core
{
    public interface IUser
    {
        string Id { get; }
    }
    public class SystemUser : IUser
    {
        public string Id { get; }

        private SystemUser(string id)
        {
            Requires.NotEmpty(id, nameof(id));

            Id = id;
        }
        
        public static IUser Create(string id) => new SystemUser(id);

        public static IUser System { get; } = Create("System");
        public static IUser Unknown { get; } = Create("Unknown");
        public static IUser Unauthorized { get; } = Create("Unauthorized");

        public override string ToString() => Id;
    }
}