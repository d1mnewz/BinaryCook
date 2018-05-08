using AutoMapper;

namespace BinaryCook.Infrastructure.AutoMapper.Mapping
{
    public abstract class Mapping<TFrom, TTo> : IMapping
    {
        public virtual IMappingExpression<TFrom, TTo> CreateMap(IMapperConfigurationExpression cfg) => cfg.CreateMap<TFrom, TTo>();

        public void Create(IMapperConfigurationExpression cfg)
        {
            CreateMap(cfg);
        }
    }
}