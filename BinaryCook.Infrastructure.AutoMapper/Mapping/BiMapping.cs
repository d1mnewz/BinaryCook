using AutoMapper;

namespace BinaryCook.Infrastructure.AutoMapper.Mapping
{
    public abstract class BiMapping<TFrom, TTo> : IMapping
    {
        public virtual IMappingExpression<TFrom, TTo> CreateForwardMap(IMapperConfigurationExpression cfg = null) => cfg?.CreateMap<TFrom, TTo>();

        public virtual IMappingExpression<TTo, TFrom> CreateBackwardMap(IMapperConfigurationExpression cfg = null) => cfg?.CreateMap<TTo, TFrom>();

        public virtual void Create(IMapperConfigurationExpression cfg)
        {
            CreateForwardMap(cfg);
            CreateBackwardMap(cfg);
        }
    }
}