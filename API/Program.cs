using API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

 builder.Services.AddControllers();

//builder.Services.AddControllers(options =>
//{
//	options.Conventions.Insert(0, new DualRoutePrefixConvention("api", "api3"));
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "API", 
        Version = "v1",
        Description = "API Documentation"
    });
    
    // Add JWT Authentication support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(jwtOptions =>
{
	jwtOptions.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])) // Use the key from appsettings.json
	};
});


// Register CORS service
builder.Services.AddCors(options =>
{
	//options.AddPolicy("AllowMyApp", policy =>
	//{
	//	policy.WithOrigins("http://localhost:3000", "file://").AllowAnyMethod().AllowAnyHeader();
	//});

	options.AddPolicy("AllowAllOrigins", policy =>
	{
		policy.AllowAnyOrigin()      // Allows all origins
			  .AllowAnyMethod()     // Allows all HTTP methods (GET, POST, etc.)
			  .AllowAnyHeader();    // Allows all headers
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAllOrigins");  // Use the CORS policy

app.UseAuthentication(); // Add this before UseAuthorization
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = string.Empty;
    c.DocumentTitle = "API Documentation";
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
