using BlackHole.DAL;
using BlackHole.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<TwitterAuthOptions>(builder.Configuration.GetSection(nameof(TwitterAuthOptions)));
builder.Services.Configure<GoogleAuthOptions>(builder.Configuration.GetSection(nameof(GoogleAuthOptions)));
// Add services to the container.
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
})
.AddGoogle(options =>
{
    options.ClientId = "586487161634-ncg9l3kg9lq11v51ucbqaveiq8dvemjq.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

builder.Services.AddSingleton<ITwitterAuthOptions>(sp => sp.GetRequiredService<IOptions<TwitterAuthOptions>>().Value);
builder.Services.AddSingleton<IGoogleAuthOptions>(sp => sp.GetRequiredService<IOptions<GoogleAuthOptions>>().Value);
builder.Services.AddTransient<ITwitterAuthService, TwitterAuthService>();
builder.Services.AddTransient<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDataContext, BHDataContext>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var dbContext = new BHDataContext();

dbContext.Database.Migrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(options =>
    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();

app.Run();
