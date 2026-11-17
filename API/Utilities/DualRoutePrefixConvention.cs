using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Utilities
{



	public class DualRoutePrefixConvention : IApplicationModelConvention
	{
		private readonly string[] _prefixes;

		public DualRoutePrefixConvention(params string[] prefixes)
		{
			_prefixes = prefixes;
		}

		public void Apply(ApplicationModel application)
		{
			foreach (var controller in application.Controllers)
			{
				var originalRoutes = controller.Selectors
											   .Where(s => s.AttributeRouteModel != null)
											   .Select(s => s.AttributeRouteModel)
											   .ToList();

				// Clear existing selectors
				controller.Selectors.Clear();

				foreach (var prefix in _prefixes)
				{
					foreach (var route in originalRoutes)
					{
						var newRoute = new AttributeRouteModel(new RouteAttribute($"{prefix}/{route.Template}"));
						controller.Selectors.Add(new SelectorModel { AttributeRouteModel = newRoute });
					}

					// If no existing routes, add default route
					if (!originalRoutes.Any())
					{
						var newRoute = new AttributeRouteModel(new RouteAttribute($"{prefix}/[controller]"));
						controller.Selectors.Add(new SelectorModel { AttributeRouteModel = newRoute });
					}
				}

				// Handle action routes
				foreach (var action in controller.Actions)
				{
					var actionRoutes = action.Selectors
										  .Where(s => s.AttributeRouteModel != null)
										  .Select(s => s.AttributeRouteModel)
										  .ToList();

					action.Selectors.Clear();

					foreach (var prefix in _prefixes)
					{
						foreach (var route in actionRoutes)
						{
							var newRoute = new AttributeRouteModel(new RouteAttribute($"{prefix}/{route.Template}"));
							action.Selectors.Add(new SelectorModel { AttributeRouteModel = newRoute });
						}

						// If no existing routes, add default route
						if (!actionRoutes.Any())
						{
							var newRoute = new AttributeRouteModel(new RouteAttribute($"{prefix}/[controller]"));
							action.Selectors.Add(new SelectorModel { AttributeRouteModel = newRoute });
						}
					}
				}
			}
		}
	}


}
