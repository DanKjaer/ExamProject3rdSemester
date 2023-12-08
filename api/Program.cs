using api;
using api.Middleware;
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
builder.Services.AddSingleton<AuthenticateRepository>();

builder.Services.AddSingleton<AnimalSpeciesService>();
builder.Services.AddSingleton<AnimalService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<SearchService>();
builder.Services.AddSingleton<HashingArgon2id>();
builder.Services.AddSingleton<AuthenticationService>();

builder.Services.AddAvatarBlobService();
builder.Services.AddJwtService();
builder.Services.AddSwaggerGenWithBearerJWT();

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

var frontendOrigin = app.Services.GetService<IConfiguration>()!["FrontendOrigin"];
app.UseCors(policy =>
    policy
        .SetIsOriginAllowed(origin => origin == "http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<JwtBearerHandler>();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();
