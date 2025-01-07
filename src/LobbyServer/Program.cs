using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ASPNETCORE_ENVIRONMENT -> builder.Environment.EnvironmentName
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services
.AddControllers(options =>
{
    // Custom request input parser �ʿ��
    // options.InputFormatters.Insert(0, new ProtoBufInputFormatter());
    // ProtoBufInputFormatter => InputFormatter ��� ����

    // Custom response parser �ʿ��
    // options.OutputFormatters.Insert(0, new ProtoBufOutputFormatter());
    // ProtoBufOutputFormatter => OutputFormatter ��� ����

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