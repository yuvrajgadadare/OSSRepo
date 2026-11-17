using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OmSaiEnvironment;
using OmSaiModels.Common;
using OmSaiModels.Worker;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using OmSaiServices.Worker.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Claims;

namespace API.Controllers
{
	[Route("api/worker")]
	[Route("api3/worker")]
	//[ApiController]
	public class ApiWorkerController : ControllerBase
	{
		private readonly WorkerService _workerService;
		private readonly WorkerAddressService _workerAddressService;
		private readonly WorkerMobileNumbersService _workerMobileNumbersService;
		private readonly WorkerDocumentService _workerDocumentService;
		private readonly SiteShiftService _siteShiftService;
		private readonly LiveDashboardService _liveDashboardService;
		private readonly WorkerShiftService _workerShiftService;



		public ApiWorkerController()
		{
			_workerService = new WorkerService();
			_workerAddressService = new WorkerAddressService();
			_workerMobileNumbersService = new WorkerMobileNumbersService();
			_workerDocumentService = new WorkerDocumentService();
			_siteShiftService = new SiteShiftService();
			_liveDashboardService = new LiveDashboardService();
			_workerShiftService = new WorkerShiftService();

		}

		[HttpGet("attendance-week-summary/{id?}")]			// siteId
		[Authorize]
		public IActionResult AttendanceWeekSummary(int id = 0)
		{
			var request = _liveDashboardService.GetWorkerAttendanceSummaryLast7Days(id == 0?null:id);

			if (request == null)
			{
				var errors = new
				{
					LeaveRequestId = new[] { "Attendance not found!" }
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));
			}
			//var type = _leaveTypeservice.GetById(id);

			return Ok(new ApiResponseModel<object>(true, request, null));
		}


		[HttpGet("site-attendance-summary/{id?}")]          // siteId [this is for supervisor summary in app]
		[Authorize]
		public IActionResult SiteAttendanceSummary(int id = 0)
		{
			var request = _liveDashboardService.GetTodayAndYesterdayAttendanceSummaryBySite(id == 0 ? null : id);

			if (request == null)
			{
				var errors = new
				{
					LeaveRequestId = new[] { "Attendance not found!" }
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));
			}
			//var type = _leaveTypeservice.GetById(id);

			return Ok(new ApiResponseModel<object>(true, request, null));
		}


		[HttpGet("get-worker-list-for-supervisor-normal/{id}")]    // siteid
		[Authorize]
		public async Task<IActionResult> WorkerListForSupervisor(int id)
		{
			try
			{
				// Get paginated results
				var result =  _workerService.WorkerListForSupervisorNormal(id);

				if (result != null || result.Any())
				{
					return Ok(new ApiResponseModel<object>(true, result, null));
				}

				var errors = new
				{
					message = new[] { "Workers not found!" }
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));

			}
			catch (Exception ex)
			{
				// Log the exception here if needed
				return BadRequest(new ApiResponseModel<object>(false, null, new
				{
					message = new[] { "An error occurred while fetching worker list." }
				}));
			}
		}





		[HttpGet("get-worker-list-for-supervisor/{id}")]    // siteid
		[Authorize]
		public async Task<IActionResult> WorkerListForSupervisor(
			int id,
			[FromQuery] int currentPage = 1,
			[FromQuery] int pageSize = 20)
		{
			try
			{
				// Validate pagination parameters
				if (currentPage < 1) currentPage = 1;
				if (pageSize < 1) pageSize = 20;
				if (pageSize > 100) pageSize = 100; // Enforce maximum page size

				// Get paginated results
				var result = await _workerService.WorkerListForSupervisor(id, currentPage, pageSize);

				if (result == null || !result.Items.Any())
				{
					return Ok(new ApiResponseModel<object>(true, new
					{
						Data = new List<object>(),
						TotalCount = 0,
						PageSize = pageSize,
						CurrentPage = currentPage,
						TotalPages = 0
					}, null));
				}

				return Ok(new ApiResponseModel<object>(true, new
				{
					Data = result.Items,
					TotalCount = result.TotalCount,
					PageSize = result.PageSize,
					CurrentPage = result.CurrentPage,
					TotalPages = result.TotalPages
				}, null));
			}
			catch (Exception ex)
			{
				// Log the exception here if needed
				return BadRequest(new ApiResponseModel<object>(false, null, new
				{
					message = new[] { "An error occurred while fetching worker list." }
				}));
			}
		}





		[HttpGet("get-workers-by-site-id/{id}")]
		[Authorize]
		public async Task<IActionResult> GetWorkersBySiteId(int id)
		{
			try
			{
				var workers = _workerService.GetAllBySiteId(id);
				if (workers == null)
				{
					var errors = new
					{
						WorkmanId = new[] { "Workers not found!" }
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));
				}

				return Ok(new ApiResponseModel<object>(true, workers, null));

			}
			catch (Exception ex)
			{
				var errors = new
				{
					message = new[] { ex.Message }
				};

				return BadRequest(new ApiResponseModel<object>(false, null, errors));

			}

		}





		[HttpGet("get-site-shift-by-site-id/{id}")]
		[Authorize]
		public async Task<IActionResult> GetSiteShiftBysiteId(int id)
		{
			try
			{
				var shifts =  _siteShiftService.GetBySiteId(id);
				if (shifts == null)
				{
					var errors = new
					{
						WorkmanId = new[] { "Site Shift not found!" }
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));
				}

				return Ok(new ApiResponseModel<object>(true, shifts, null));

			}
			catch (Exception ex)
			{
				var errors = new
				{
					message = new[] { ex.Message }
				};

				return BadRequest(new ApiResponseModel<object>(false, null, errors));

			}

		}


		//GetAllBySiteId


		[HttpGet("get-site-shift/{id}")]
		[Authorize]
		public async Task<IActionResult> GetSiteShift(int id)
		{
			try
			{

				var worker = await _workerService.GetSiteShifts(id);
				if (worker == null)
				{
					var errors = new
					{
						WorkmanId = new[] { "Worker Site Shift not found!" }
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));
				}

				return Ok(new ApiResponseModel<object>(true, worker, null));

			}
			catch (Exception ex)
			{
				var errors = new
				{
					message = new[] { ex.Message }
				};

				return BadRequest(new ApiResponseModel<object>(false, null, errors));

			}

		}


		[HttpPost]
		[Route("change-worker-shift")]
		[Authorize] // Add appropriate authorization
		public async Task<IActionResult> ChangeWorkerShift([FromBody] ChangeShiftRequest request)
		{
			try
			{
				// Validate input
				if (request.WorkerId <= 0 || request?.ShiftId <= 0)
				{
					return BadRequest(new
					{
						success = false,
						data = (object)null,
						errors = new
						{
							message = "Invalid worker ID or shift ID"
						}
					});

				}

				// Verify worker exists
				var workerExists = await _workerService.ExistsAsync(request.WorkerId);
				if (!workerExists)
				{
					return NotFound(new
					{
						success = false,
						data = (object)null,
						errors = new
						{
							message = "Worker not found"
						}
					});
				}

				// Verify shift exists
				var shiftExists = await _workerShiftService.ExistsAsync(request.ShiftId);
				if (!shiftExists)
				{
					return NotFound(new
					{
						success = false,
						data = (object)null,
						errors = new
						{
							message = "Shift not found"
						}
					});

				}

				//// Check authorization - example using current user
				//var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				//if (!await _authService.CanChangeWorkerShift(currentUserId, workerId))
				//{
				//	return Forbid();
				//}


				WorkerShiftModel workerShiftModel = new WorkerShiftModel
				{
					WorkerId = request.WorkerId,
					SiteShiftId = request.ShiftId
				};
				// Attempt to change the password using the service method
				var result =  _workerShiftService.Create(workerShiftModel);

				return Ok(new
				{
					success = true,
					data = "record updated successfully.",
					errors = (object)null
				});

			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = ex.Message
					}
				});
			}
		}


		[HttpGet("profile/{id}")]
		[Authorize]
		public IActionResult GetProfile(int id)
		{
			var worker = _workerService.GetProfileById(id, null);
			if (worker == null)
			{
				var errors = new
				{
					WorkmanId = new[] { "Worker not found!" }
				};
				return Unauthorized(new ApiResponseModel<object>(false, null, errors));
			}
			var documents = _workerDocumentService.GetAll(id);
			var address = _workerAddressService.GetByWorkerId(id);

			return Ok(new ApiResponseModel<object>(true, new
			{
				worker,
				address,
				documents,
			}, null));

		}

		[HttpPost("upload-profile-image")]
		[Authorize]
		public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile? file, [FromForm] int workerId, [FromForm] string? profileImage = "")
		{
			string uploadPath = "media/profile_images";

			var model = new ProfileImageModel
			{
				WorkerId = workerId,
				ProfileImage = profileImage
			};

			try
			{
				var worker = _workerService.GetById(workerId);
				if (worker == null)
				{
					var errors = new
					{
						WorkerId = new[] { "Worker not found!" },
						File = file == null ? new[] { "No file uploaded!" } : null,
						ProfileImage = string.IsNullOrEmpty(profileImage) ? new[] { "Profile image path is missing!" } : null
					};

					return BadRequest(new ApiResponseModel<object>(false, null, errors));
				}

				if (file != null && file.Length > 0)
				{
					var uploadUrl = $"{Urls.mvcURL}/api/external/upload-file"; // MVC endpoint to handle file saving

					using (var content = new MultipartFormDataContent())
					{
						var streamContent = new StreamContent(file.OpenReadStream());
						content.Add(streamContent, "file", file.FileName);

						// Add the path as StringContent
						var pathContent = new StringContent("/profile_images");
						content.Add(pathContent, "path");


						using (var client = new HttpClient())
						{
							var response = await client.PostAsync(uploadUrl, content);
							if (response.IsSuccessStatusCode)
							{

								string ProfileImage = await response.Content.ReadAsStringAsync();

								_workerService.UploadProfileImage(workerId, ProfileImage);

								return Ok(new ApiResponseModel<object>(true, new
								{
									message = "Profile image uploaded successfully!",
									filePath = ProfileImage
								}, null));
							}
							else
							{
								var errors = new
								{
									message = $"An error occurred: {await response.Content.ReadAsStringAsync()}"
								};

								return BadRequest(new ApiResponseModel<object>(false, null, errors));
							}
						}
					}

				}
				else
				{
					var errors = new
					{
						File = file == null ? new[] { "Image not found!" } : null,
					};

					return BadRequest(new ApiResponseModel<object>(false, null, errors));

				}

			}
			catch (Exception ex)
			{
				var errors = new
				{

					message = $"An error occurred: {ex.Message}"

				};

				return BadRequest(new ApiResponseModel<object>(false, null, errors));

			}


		}


		[HttpPost("upload-document")]
		public IActionResult UploadDocument([FromForm] IFormFile? file, [FromForm] int WorkerId, [FromForm] int DocumentId, [FromForm] string? documentNumber = "", string? documentImage = null)
		{
			string uploadPath = "media/documents";

			var model = new WorkerDocumentModel
			{
				DocumentId = DocumentId,
				DocumentNumber = documentNumber,
				WorkerId = WorkerId

			};
			try
			{
				var worker = _workerService.GetById(WorkerId);
				if (worker == null)
				{
					var errors = new
					{
						WorkerId = new[] { "Worker not found!" },
						File = file == null ? new[] { "No file uploaded!" } : null,
						DocumentId = DocumentId <= 0 ? new[] { "Invalid DocumentId!" } : null
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));


				}
				if (file != null && file.Length > 0)
				{

					var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);


					if (!Directory.Exists(documentPath))
						Directory.CreateDirectory(documentPath);


					var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
					var filePath = Path.Combine(documentPath, uniqueFileName);


					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					model.DocumentImage = $"{uploadPath}/{uniqueFileName}";
					_workerDocumentService.Update(model);

					return Ok(new ApiResponseModel<object>(true, new

					{

						message = "Document uploaded successfully!",
						filePath = $"{uploadPath}/{uniqueFileName}"
					}, null));
				}
				else
				{
					model.DocumentImage = documentImage;

					_workerDocumentService.Update(model);
					return Ok(new ApiResponseModel<object>(true, new

					{

						message = "Document updated successfully!",
						filePath = documentImage
					}, null));


				}

			}
			catch (Exception ex)
			{
				var errors = new
				{
					message = $"An error occurred: {ex.Message}"
				};

				return BadRequest(new ApiResponseModel<object>(false, null, errors));

			}
		}


	}
}
