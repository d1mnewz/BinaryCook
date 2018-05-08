using AutoMapper;

namespace BinaryCook.Infrastructure.AutoMapper.Mapping
{
    public interface IMapping
    {
        void Create(IMapperConfigurationExpression cfg);
    }
}