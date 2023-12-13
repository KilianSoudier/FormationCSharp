using Formation.Data;
using Formation.Data.Entities;
using Formation.DTO.Login;
using Formation.DTO.Prescripteur;
using Formation.Middlewares;
using Formation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using NuGet.Protocol;
using RulesEngine.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(arg =>
    {
        if (builder.Environment.IsDevelopment())
        {
            arg.EnableSensitiveDataLogging(true);
            var dbcs = builder.Configuration.GetConnectionString("MainDb");// Viens chercher dans le app settings.json
            arg.UseSqlServer(dbcs);
        }
    });
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //Ajoute l'authentification par JWT
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters() //Paramètres pour valider le token, qui sont tous par défault sur false.
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],   //Viens chercher dans le appsettings.json : Jwt dans son attribut Audience
                ValidIssuer = builder.Configuration["Jwt:Issuer"],        //Les __ sont comme les : sauf qu'ils sont compatibles pour linux contrairement au :
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)), //Transforme en tableau de bytes comme demandé par la symmetricsecuritykey
                                                                                //Le ! au dessus permet de retirer le warning car il y a un risque de null, on lui indique qu'on est sûr que ce ne sera pas null
            };
        });
    builder.Services.AddAuthorization();
    builder.Services.AddScoped<IPrescripteurService, PrescripteurService>();  //Se refait à chaque appel
    builder.Services.AddScoped<IEchantillonService, EchantillonService>();  //Se refait à chaque appel

    builder.Services.AddSingleton<WorkflowsObject>(svc =>
    {
        var result = new WorkflowsObject();
        try
        {
            var file = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "rules.json"
                );
            var json = File.ReadAllText(file);
            result.Workflows = JsonSerializer.Deserialize<List<Workflow>>(json);
        }
        catch (Exception)
        {
            result.Workflows = new List<Workflow>();
        }
        return result;
    });
    builder.Services.AddScoped(typeof(IFormationRulesEngine<>), typeof(FormationRulesEngine<>));
    builder.Services.AddHostedService<BackgroundJob>(); //Ajoute la tâche répétitive pour toute la durée du projet.
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();//Permet d'éviter de faire un try catch par fonction du controller

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())                //-----------------------Github.dev a la place de .com ouvre un vs code en ligne
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGet("/{id:int}/ready/{isReady:bool}", (int id, bool isReady) => $"Bonjour Monde! {id} ({isReady}), ");    //Créé une route sur la racine : Lien + /1 qui retourne bonjour monde son id et is ready

    var preGrp = app.MapGroup("minapi/prescripteur");  //Permet de grouper les mapPost
    preGrp.WithOpenApi().WithDescription("MEUSE")   //Ajoute MEUSE en description dans la route la sur swagger
    .MapPost("", async (PostPrescripteurDto p, IPrescripteurService svc) =>  //"" renvoie vers l'url a la racine url+/
    {
        await svc.PostAsync(p);
        return p.Denomination;
    });

    app.MapGet("/auth", () => "You are auth")
        .RequireAuthorization();    //Si le token est valide par défault sinon il faut rajouter des authorization policies
                                    //https://gist.github.com/AwassifPro/adab39cdcdd8318510293f210c48b95c Pour avoir la connexion à ActiveDirectory
    app.MapPost("/auth", async (LoginDto credentials, IAuthService svc) =>
    {
        var profile = await svc.AuthenticateUserAsync(credentials.Username, credentials.Password);
        if (profile == null)
        {
            return Results.Problem(detail: "On ne veut pas de toi", title: "Casse toi!", statusCode:StatusCodes.Status403Forbidden);//Forbidden si pas Auth
        }                                                                                                                           //Unauthorised si pas les Droits
        var token = svc.GetToken(profile!);
        return Results.Ok(token);
    });

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}