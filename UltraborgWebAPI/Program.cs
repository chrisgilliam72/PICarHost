var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var ultraBorg = new Ultraborg.Library.Ultraborg();
int address = ultraBorg.GetUltraBorgAdress();
ultraBorg.Init(1, address);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/Distance/{sensorNo}", (int sensorNo) =>
{

    double distance = sensorNo switch
    {
        1 => ultraBorg.GetFilteredDistance1(),
        2 => ultraBorg.GetFilteredDistance2(),
        3 => ultraBorg.GetFilteredDistance3(),
        4 => ultraBorg.GetFilteredDistance4(),
        _ => -1

    };

    return distance;
}).WithName("GetDistance").WithOpenApi();
app.Run();

