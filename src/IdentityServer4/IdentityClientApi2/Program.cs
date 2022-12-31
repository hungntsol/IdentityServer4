var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication("JwtBearer")
	.AddJwtBearer("JwtBearer", options =>
	{
		const string server = "https://localhost:15000";
		options.Authority = server;
		options.Audience = "example.api.v1";
	});

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("IdentityServer",
	httpClient => { httpClient.BaseAddress = new Uri("https://localhost:15000/"); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
