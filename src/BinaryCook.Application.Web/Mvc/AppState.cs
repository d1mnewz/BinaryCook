using System.Security.Claims;

namespace BinaryCook.Application.Web.Mvc
{
	public class AppState
	{
		//TODO:
		public bool IsAuthenticated => true;


		public AppState(ClaimsPrincipal user)
		{
		}
	}
}