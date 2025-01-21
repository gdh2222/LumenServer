using DBMediator.Contexts;
using DBMediator.Models;
using Newtonsoft.Json.Serialization;
using ServerCommon;

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


    // registe custom exception filter
    options.Filters.Add<LunarExceptionFilter>();
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


builder.Services.AddScoped<DbContextFactory>();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Set GlobalConfig
VersionChecker.GlobalConfig.CDN_Prefix = builder.Configuration.GetValue("CDNUrl", string.Empty)!;

// Setup DBConfig
// DBMediator.Contexts.DbConfig.Instance.Setup(builder.Configuration);



var app = builder.Build();


DbConfig.Instance.Setup(builder.Configuration.GetConnectionString("AccountDBString")!);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
