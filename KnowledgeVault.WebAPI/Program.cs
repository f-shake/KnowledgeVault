using KnowledgeVault.WebAPI;
using KnowledgeVault.WebAPI.Service;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

string cors = "CorsPolicy";
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<KnowledgeVaultDbContext>();
builder.Services.AddTransient<AchievementService>();
builder.Services.AddTransient<PropertyService>();
builder.Services.AddTransient<FileService>();
builder.Services.AddTransient<ArchiveService>();
builder.Services.AddTransient<KnowledgeVaultActionFilter>();

IServiceProvider serviceProvider = null;

// Add services to the container.

builder.Services.AddControllers(o =>
{
    o.Filters.Add(serviceProvider.GetRequiredService<KnowledgeVaultActionFilter>());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(p =>
{
    var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
    var xmlPath = Path.Combine(basePath, "KnowledgeVault.WebAPI.xml");
    p.IncludeXmlComments(xmlPath);



    var scheme = new OpenApiSecurityScheme()
    {
        Description = "Authorization header",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Authorization"
        },
        Scheme = "oauth2",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    };
    p.AddSecurityDefinition("Authorization", scheme);
    var requirement = new OpenApiSecurityRequirement();
    requirement[scheme] = new List<string>();
    p.AddSecurityRequirement(requirement);
});
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(policy =>
{

    policy.AddPolicy(cors, opt => opt
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("X-Pagination"));

});

builder.Services.Configure<IISServerOptions>(p =>
{
    p.MaxRequestBodySize = int.MaxValue;
});
builder.Services.Configure<KestrelServerOptions>(p =>
{
    p.Limits.MaxRequestBodySize = int.MaxValue;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue; 
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

var app = builder.Build();
serviceProvider = app.Services;

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseSession();
app.UseCors(cors);
app.UseAuthorization();

app.MapControllers();


app.Run();
