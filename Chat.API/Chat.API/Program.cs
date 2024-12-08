using System.Text;
using Chat.API.Data;
using Chat.API.Hubs;
using Chat.API.Interfaces;
using Chat.API.Middlewares;
using Chat.API.Options;
using Chat.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CorsOptions = Chat.API.Options.CorsOptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

# region Configure Authentication
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Authorization token"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var jwtOptions = new JwtOptions();
builder.Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions); 

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true,
        ValidIssuer = jwtOptions.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
    };
});
# endregion Configure Authentication

# region Configure Services
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection(nameof(AuthOptions)));
builder.Services.Configure<CorsOptions>(builder.Configuration.GetSection(nameof(CorsOptions)));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IConnectionManager, ConnectionManager>();
builder.Services.AddScoped<IClaimsManager, ClaimsManager>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddSingleton<ExceptionMiddleware>();
# endregion Configure Services

builder.Services.AddSignalR();
builder.Services.AddControllers();

# region Configure Database
var dataContextOptions = new DataContextOptions();
builder.Configuration.GetSection(nameof(DataContextOptions)).Bind(dataContextOptions);
builder.Services.AddDataContext(dataContextOptions);
# endregion Configure Database

var corsOptions = new CorsOptions();
builder.Configuration.GetSection(nameof(CorsOptions)).Bind(corsOptions);
builder.Services.AddCors(opt
    => opt.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins(corsOptions.AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("chat-hub");

app.Run();