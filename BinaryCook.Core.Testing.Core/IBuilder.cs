namespace BinaryCook.Core.Testing.Core
{
	public interface IBuilder<out T>
	{
		T Build();
	}
}