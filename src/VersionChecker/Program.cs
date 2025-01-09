using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ASPNETCORE_ENVIRONMENT -> builder.Environment.EnvironmentName
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services
.AddControllers(options => 
{
    // Custom request input parser 필요시
    // options.InputFormatters.Insert(0, new ProtoBufInputFormatter());
    // ProtoBufInputFormatter => InputFormatter 상속 구현

    // Custom response parser 필요시
    // options.OutputFormatters.Insert(0, new ProtoBufOutputFormatter());
    // ProtoBufOutputFormatter => OutputFormatter 상속 구현

})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
})
.AddJsonOptions(options => 
{
    // Set PascalCase
    options.JsonSerializerOptions.PropertyNamingPolicy = null; 
});



// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Set GlobalConfig
VersionChecker.GlobalConfig.CDNUrl = builder.Configuration.GetValue("CDNUrl", string.Empty)!;

// Setup DBConfig
DBMediator.Contexts.DbConfig.Instance.Setup(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
