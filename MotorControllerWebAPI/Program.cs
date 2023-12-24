var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("/back", () =>
{

})
.WithName("back")
.WithOpenApi();

app.MapGet("/forward", () =>
{

})
.WithName("forward")
.WithOpenApi();

app.MapGet("/left", () =>
{

})
.WithName("left")
.WithOpenApi();

app.MapGet("/right", () =>
{

})
.WithName("right")
.WithOpenApi();

app.MapGet("/stop", () =>
{

})
.WithName("stop")
.WithOpenApi();

app.MapGet("/faster", () =>
{

})
.WithName("faster")
.WithOpenApi();

app.MapGet("/slower", () =>
{

})
.WithName("slower")
.WithOpenApi();

app.Run();


