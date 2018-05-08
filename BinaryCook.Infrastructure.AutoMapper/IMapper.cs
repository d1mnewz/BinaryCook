using AutoMapper;

namespace BinaryCook.Infrastructure.AutoMapper
{
	public interface IMapper<in TFrom, out TTo>
	{
		TTo Map(TFrom from);
	}

	public class Mapper<TFrom, TTo> : IMapper<TFrom, TTo>
	{
		private readonly IMapper _mapper;

		public Mapper(IMapper mapper)
		{
			_mapper = mapper;
		}

		public TTo Map(TFrom from) => _mapper.Map<TTo>(from);
	}
}