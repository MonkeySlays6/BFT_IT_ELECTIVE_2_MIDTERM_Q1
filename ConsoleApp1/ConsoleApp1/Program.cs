using ConsoleApp1.DTOs;
using ConsoleApp1.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the Container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your custom UserService so it can be injected into endpoints
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// 2. Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. Define Endpoints

// POST: /api/auth/register
app.MapPost("/api/auth/register", (RegisterUserDto dto, UserService userService) =>
{
    if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
    {
        return Results.BadRequest(new { message = "Username and password are required." });
    }

    var createdUser = userService.RegisterUser(dto);

    // Return 201 Created with the resulting user profile
    return Results.Created($"/api/users/{createdUser.Id}/profile", new UserProfileDto
    {
        UserId = createdUser.Id,
        Username = createdUser.Username,
        TrainerName = createdUser.Profile.TrainerName,
        FavoritePokemon = createdUser.Profile.FavoritePokemon,
        AvatarUrl = createdUser.Profile.AvatarUrl
    });
});

// GET: /api/users/{id}/profile
app.MapGet("/api/users/{id:int}/profile", (int id, UserService userService) =>
{
    var profile = userService.GetUserProfile(id);

    if (profile is null)
    {
        return Results.NotFound(new { message = $"User profile for ID {id} not found." });
    }

    return Results.Ok(profile);
});

app.Run();