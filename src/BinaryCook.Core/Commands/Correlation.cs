using System;

namespace BinaryCook.Core.Commands
{
    public interface ICorrelation
    {
        string Id { get; }
        string ConversationId { get; }
    }

    public class Correlation : ICorrelation
    {
        public string Id { get; }
        public string ConversationId { get; }

        public Correlation() : this(Guid.NewGuid().ToString("N"))
        {
        }

        public Correlation(string id) : this(id, id)
        {
        }

        public Correlation(string id, string conversationId)
        {
            Id = id;
            ConversationId = conversationId;
        }
    }
}