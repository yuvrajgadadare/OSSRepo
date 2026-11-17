using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Route("api3/[controller]")]
	public class TestController : ControllerBase
	{
		[HttpGet]
		public ActionResult Index()
		{
			return Ok(new { message = "This is Test API" });
		}

		[HttpGet("employee-data/{id}")]
		public ActionResult employeesData(int id)
		{

			var dummyEmployees = Enumerable.Range(1, id).Select(i => new
			{
				id = i,
				employeeId = $"EMP{i.ToString().PadLeft(4, '0')}",
				name = $"Employee {i}",
				department = i % 5 == 0 ? "HR" :
							   i % 4 == 0 ? "Finance" :
							   i % 3 == 0 ? "IT" :
							   i % 2 == 0 ? "Operations" : "Sales",
				position = i % 5 == 0 ? "Manager" :
							 i % 4 == 0 ? "Supervisor" :
							 i % 3 == 0 ? "Developer" :
							 i % 2 == 0 ? "Analyst" : "Associate",
				salary = 30000 + (i * 500),
				joinDate = DateTime.Now.AddMonths(-i).ToString("yyyy-MM-dd"),
				email = $"employee{i}@company.com",
				phone = $"555-{i.ToString().PadLeft(4, '0')}",
				isActive = i % 10 != 0 // Every 10th employee is inactive
			}).ToList();

			return Ok(new
			{
				success = true,
				data = dummyEmployees,
				errors = (object)null
			});
		}

	}
}
