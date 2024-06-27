using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using WebApi.Models;
using WebApi.Models.LabWorks;
using WebApi.Models.WebsocketProxies;
using WebApi.Services;
using WebApi.Services.Logs;
using WebApi.Services.Logs.LogsParsers;
using WebApi.Services.Logs.LogsParsers.TerminalLogsParser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<User>("users"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<News>("news"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<Vm>("vms"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<Lab>("labs"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<LabEntity>("labsEntity"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<LabReservation>("labReservations"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<LabWork>("labWorks"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<InstructionStep>("instructionStep"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<LabWorkInstruction>("labWorkInstruction"));
builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017").GetDatabase("rtf-db")
    .GetCollection<UserLabResult>("userLabResult"));

builder.Services.AddSingleton<VmService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<NewsService>();
builder.Services.AddSingleton<ProxmoxService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<LabsService>();
builder.Services.AddSingleton<LabReservationsService>();
builder.Services.AddSingleton<LabWorkService>();
builder.Services.AddSingleton<WebsocketProxyService, WebsocketProxyService>();
builder.Services.AddSingleton<VirtualDesktopService, VirtualDesktopService>();
builder.Services.AddSingleton<WebsocketProxySettings, WebsocketProxySettings>(x => new WebsocketProxySettings()
    {
        WebsocketHost = builder.Configuration["WebsocketProxy:WebsocketHost"],
        ProxmoxVncStartingPort = int.Parse(builder.Configuration["WebsocketProxy:ProxmoxVncStartingPort"]),
        Path = builder.Configuration["WebsocketProxy:Path"],
    });
builder.Services.AddSingleton<ILogsParser, TerminalAuditLogsParser>();
builder.Services.AddSingleton<LogsReader>();
builder.Services.AddSingleton<InstructionStepsService>();
builder.Services.AddSingleton<LabWorkInstructionService>();
builder.Services.AddSingleton<UserLabResultsService>();

builder.Services.AddCors(p => p.AddPolicy("AllowAll",
    b => { b.WithOrigins("http://10.40.229.60:3000","http://localhost:5173", "http://localhost:4173", "http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials(); }));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("Jwt:Key").Value!))
    };
});

builder.Services.AddSwaggerGen(options => { options.SupportNonNullableReferenceTypes(); });

var app = builder.Build();

app.UseAuthentication();
app.UseCors("AllowAll");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();