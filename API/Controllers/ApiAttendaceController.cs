using Azure.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using LmsServices.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmSaiEnvironment;
using OmSaiModels.Common;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using OmSaiServices.Worker.Interfaces;
using SixLabors.ImageSharp;
using System.Net.Http;

namespace API.Controllers
{
	[Route("api/attendance")]
	[Route("api3/attendance")]
	//[ApiController]
	//[Authorize]
	public class ApiAttendaceController : ControllerBase
	{
		private readonly WorkerService _workerService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly MultiMediaService _mms;

		public ApiAttendaceController()
		{
			_workerService = new WorkerService();
			_attendanceService = new WorkerAttendanceService();
			_mms = new MultiMediaService();

		}


		[HttpPost("worker-attendance-report")]
		public async Task<IActionResult> WorkerAttendanceReport([FromBody] WorkerAttendanceFilter request)
		{
			if (request == null)
			{
				return BadRequest("Invalid request payload");
			}
			// Validate the parameters
			// Validate that at least one of the parameters is provided
			if (!request.WorkerId.HasValue && !request.SiteId.HasValue && !request.CurrentDate.HasValue && !request.Month.HasValue && !request.Year.HasValue)
			{
				return BadRequest(new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = "At least one parameter (WorkerId, SiteId, Month, Year or CurrentDate) must be provided."
					}
				});
			}

			try
			{
				// Retrieve attendance history from the service
				var attendanceHistory = _attendanceService.GetAll(request);

				// Return a successful response
				return Ok(new
				{
					success = true,
					data = attendanceHistory,
					errors = (object)null
				});
			}
			catch (Exception ex)
			{
				// Handle exceptions
				return BadRequest(new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = "An error occurred while processing the request.",
						details = ex.Message
					}
				});
			}
		}


		[HttpGet("site-worker-attendance-log/{id}")]
		public async Task<IActionResult> SiteWorkerAttendanceLog(int id)
		{

			if (id == null)
			{
				return BadRequest("Invalid SiteId");
			}

			try
			{
				WorkerAttendanceFilter request = new WorkerAttendanceFilter()
				{
					SiteId = id,
					StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),  
					EndDate = DateOnly.FromDateTime(DateTime.Today),  
				};

				// Validate the parameters
				if (!request.SiteId.HasValue)
				{
					return BadRequest(new
					{
						success = false,
						data = (object)null,
						errors = new
						{
							message = "SiteId must be provided."
						}
					});
				}

				// Retrieve attendance history from the service
				var attendanceHistory = _attendanceService.GetAll(request);

				// Return a successful response
				return Ok(new
				{
					success = true,
					data = attendanceHistory,
					errors = (object)null
				});
			}
			catch (Exception ex)
			{
				// Handle exceptions
				return BadRequest(new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = "An error occurred while processing the request.",
						details = ex.Message
					}
				});
			}
		}





		[HttpPost("worker-selfie-attendance")]
		public async Task<IActionResult> WorkerSelfieAttendance([FromForm] WorkerAttendanceModel model, IFormFile SelfieImage)
		{
			try
			{
				// Validate the model
				if (!ModelState.IsValid)
				{
					var errors = ModelState
						   .Where(x => x.Value.Errors.Count > 0)
						   .ToDictionary(
							   kvp => kvp.Key,
							   kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
						   );

					var response = new
					{
						success = false,
						data = (object)null,
						errors
					};

					return BadRequest(response);

				}

				// Check if an image is provided
				if (SelfieImage == null || SelfieImage.Length == 0)
				{

					var errors = new
					{
						SelfieImage = new[] { "Selfie image is required!" }
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));
				}

				try
				{
					// Image compression
					// var uploadPath = "selfies"; // Define your folder path
					// var maxFileSizeKb = 30; // 30KB size limit
					// var quality = 20;
					// var filePath = await _mms.SaveAndCompressImageAsync(SelfieImage, uploadPath, maxFileSizeKb, quality);


					var uploadUrl = $"{Urls.mvcURL}/api/external/upload-file"; // MVC endpoint to handle file saving

					using (var content = new MultipartFormDataContent())
					{

						var streamContent = new StreamContent(SelfieImage.OpenReadStream());
						content.Add(streamContent, "file", SelfieImage.FileName);

						// Add the path as StringContent
						var pathContent = new StringContent("/selfies");
						content.Add(pathContent, "path");

						using (var client = new HttpClient())
						{
							var response = await client.PostAsync(uploadUrl, content);
							if (response.IsSuccessStatusCode)
							{

								string selfieImage = await response.Content.ReadAsStringAsync();

								model.SelfieImage = selfieImage;

								_attendanceService.ManageAttendance(model);

								return Ok(new ApiResponseModel<object>(true, new
								{
									message = "Attendance processed successfully!",
									filePath = selfieImage
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
				catch (Exception ex)
				{

					var errors = new
					{
						SelfieImage = new[] { ex.Message }
					};
					return Unauthorized(new ApiResponseModel<object>(false, null, errors));
				}

			}
			catch (Exception ex)
			{
				var errors = new
				{
					SelfieImage = new[] { ex.Message }
				};
				return Unauthorized(new ApiResponseModel<object>(false, null, errors));
			}
		}


		[HttpPost("worker-selfie-attendance2")]
		public async Task<IActionResult> WorkerSelfieAttendance2([FromForm] WorkerAttendanceModel model, IFormFile SelfieImage)
		{
			try
			{
				// Validate the model
				if (!ModelState.IsValid)
				{
					var errors = ModelState
						.Where(x => x.Value.Errors.Count > 0)
						.ToDictionary(
							kvp => kvp.Key,
							kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
						);

					return BadRequest(new ApiResponseModel<object>(false, null, errors));
				}

				// Check if a selfie image is provided
				if (SelfieImage == null || SelfieImage.Length == 0)
				{
					return BadRequest(new ApiResponseModel<object>(false, null, new { SelfieImage = new[] { "Selfie image is required!" } }));
				}

				try
				{
					using (var client = new HttpClient())
					{
						// Upload the selfie image
						string uploadUrl = $"{Urls.mvcURL}/api/external/upload-file"; // MVC endpoint for file saving

						using (var content = new MultipartFormDataContent())
						{
							content.Add(new StreamContent(SelfieImage.OpenReadStream()), "file", SelfieImage.FileName);
							content.Add(new StringContent("/selfies"), "path");

							var uploadResponse = await client.PostAsync(uploadUrl, content);
							if (!uploadResponse.IsSuccessStatusCode)
							{
								var errorMessage = await uploadResponse.Content.ReadAsStringAsync();
								return BadRequest(new ApiResponseModel<object>(false, null, new { message = $"Image upload failed: {errorMessage}" }));
							}

							string selfieImage = await uploadResponse.Content.ReadAsStringAsync();
							var worker = _workerService.GetById(model.WorkerId ?? 0);
							var test1 = worker;
							var test2 = worker;
							string profileImage = worker?.ProfileImage ?? "";

							if (string.IsNullOrEmpty(profileImage))
							{
								return BadRequest(new ApiResponseModel<object>(false, null, new { message = "Please upload your profile image first!" }));
							}

							//// Compare the profile image with the newly uploaded selfie (POST)
							//string compareUrl = $"{Urls.mvcURL}/compare-faces";
							//var formData = new MultipartFormDataContent
							//{
							//	{ new StringContent(profileImage), "imagePath1" },
							//	{ new StringContent(selfieImage), "imagePath2" }
							//};

							//var compareResponse = await client.PostAsync(compareUrl, formData);
							//var responseBody = await compareResponse.Content.ReadAsStringAsync();

							//if (!compareResponse.IsSuccessStatusCode)
							//{
							//	return BadRequest(new ApiResponseModel<object>(false, null, new { message = "Face comparison API request failed." }));
							//}

							// Compare the profile image with the newly uploaded selfie using GET request
							string compareUrl = $"{Urls.mvcURL}/compare-faces?imagePath1={Uri.EscapeDataString(profileImage)}&imagePath2={Uri.EscapeDataString(selfieImage)}";

							var compareResponse = await client.GetAsync(compareUrl);
							var responseBody = await compareResponse.Content.ReadAsStringAsync();

							if (!compareResponse.IsSuccessStatusCode)
							{
								return BadRequest(new ApiResponseModel<object>(false, null, new { message = "Face comparison API request failed." }));
							}

							// Deserialize the API response
							var faceCompareResult = JsonConvert.DeserializeObject<FaceCompareResponse>(responseBody);
							if (faceCompareResult == null || !faceCompareResult.Success)
							{
								return BadRequest(new ApiResponseModel<object>(false, null, new { message = "Invalid response from face comparison API." }));
							}

							// Check if face matches
							if (!faceCompareResult.IsMatch)
							{
								return BadRequest(new ApiResponseModel<object>(false, null, new { message = "Face comparison failed. Selfie does not match profile image." }));
							}

							// Save attendance
							model.SelfieImage = selfieImage;
							_attendanceService.ManageAttendance(model);

							return Ok(new ApiResponseModel<object>(true, new
							{
								message = "Attendance processed successfully!",
								filePath = selfieImage
							}, null));
						}
					}
				}
				catch (Exception ex)
				{
					return StatusCode(500, new ApiResponseModel<object>(false, null, new { error = ex.Message }));
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new ApiResponseModel<object>(false, null, new { error = ex.Message }));
			}
		}

		// DTO for Face Comparison Response
		public class FaceCompareResponse
		{
			public bool Success { get; set; }
			public bool IsMatch { get; set; }
		}



		[HttpGet("validate-attendance/{workmanId}")]
		public IActionResult ValidateAttendance(string workmanId)
		{
			try
			{
				var worker = _workerService.GetProfileById(null, workmanId);
				if (worker == null || workmanId == null)
				{
					return BadRequest(new
					{
						success = false,
						data = (object)null,
						errors = new
						{
							message = "Invalid Worker!"
						}
					});
				}

				ValidateAttendanceModel validateData = _attendanceService.ValidateAttendance(worker.WorkerId);


				// Return a successful response
				return Ok(new
				{
					success = true,
					data = validateData,
					errors = (object)null
				});

			}
			catch (Exception ex)
			{
				// Handle exceptions
				return StatusCode(500, new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = "An error occurred while processing the request.",
						details = ex.Message
					}
				});
			}
		}


		[HttpGet("validate-attendance2/{workmanId}")]
		public IActionResult ValidateAttendance2(string workmanId)
		{
			try
			{
				var worker = _workerService.GetProfileById(null, workmanId);
				if (worker == null || workmanId == null)
				{
					return BadRequest(new
					{
						success = false,
						data = (object)null,
						errors = new
						{
							message = "Invalid Worker!"
						}
					});
				}

				ValidateAttendanceModel validateData = _attendanceService.ValidateAttendance2(worker.WorkerId);


				// Return a successful response
				return Ok(new
				{
					success = true,
					data = validateData,
					errors = (object)null
				});

			}
			catch (Exception ex)
			{
				// Handle exceptions
				return StatusCode(500, new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = "An error occurred while processing the request.",
						details = ex.Message
					}
				});
			}
		}



	}

}
