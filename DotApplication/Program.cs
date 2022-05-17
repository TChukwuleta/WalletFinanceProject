using Dot.Application.Interfaces;
using Dot.Application.UserCommand.CreateUser;
using Dot.Core.Entities;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Application.BarcodeCommand.Commands;
using Dot.Infrastructure.Data;
using Dot.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
/*builder.Services.AddMediatR(typeof(Program).Assembly);*/
builder.Services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);
builder.Services.AddMediatR(typeof(GenerateBarcodeCommandHandler).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
/*builder.Services.AddSwaggerGen();*/
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["TokenConstants:key"]);
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        RequireExpirationTime = false
    };
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));


builder.Services.AddScoped<IRepository, EntityRepository>();
builder.Services.AddScoped<IRepositoryReadOnly, EntityRepositoryReadOnly>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IUploadService, UploadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/*app.UseSwagger();
app.UseSwaggerUI();*/

/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
    //endpoints.MapRazorPages();
});*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
