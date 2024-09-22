using JSON;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var jsonOptions = builder.Services.AddJsonOptions();

builder.Services.AddControllers().AddJsonOptions(config =>
{
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonOptions.PropertyNameCaseInsensitive;
    config.JsonSerializerOptions.DefaultIgnoreCondition = jsonOptions.DefaultIgnoreCondition;
    config.JsonSerializerOptions.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;

    config.JsonSerializerOptions.Converters.Clear();

    foreach (var converter in jsonOptions.Converters)
    {
        config.JsonSerializerOptions.Converters.Add(converter);
    }
});

builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
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
