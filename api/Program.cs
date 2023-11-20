using infrastructure;
using infrastructure.Repositories;
using service.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());


// Add services to the container.
builder.Services.AddSingleton<AnimalSpeciesRepository>();
builder.Services.AddSingleton<AnimalsRepository>();
builder.Services.AddSingleton<AnimalSpeciesService>();
builder.Services.AddSingleton<AnimalService>();
builder.Services.AddControllers();
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

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
