using infrastructure;
using infrastructure.Repositories;
using service.PasswordHashing;
using service.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());


// Add services and repositories to the container.
builder.Services.AddSingleton<AnimalSpeciesRepository>();
builder.Services.AddSingleton<AnimalsRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<SearchRepository>();

builder.Services.AddSingleton<AnimalSpeciesService>();
builder.Services.AddSingleton<AnimalService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<SearchService>();
builder.Services.AddSingleton<HashingArgon2id>();

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
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
