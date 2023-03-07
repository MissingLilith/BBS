using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolBBS.Entity.Models;
using SchoolBBS.Server;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSwaggerGen(c =>
{
    //Bearer ��scheme����
    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        //���������ͷ��
        In = ParameterLocation.Header,
        //ʹ��Authorizeͷ��
        Type = SecuritySchemeType.Http,
        //����Ϊ�� bearer��ͷ
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    //�����з�������Ϊ����bearerͷ����Ϣ
    var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearerAuth"
                                }
                            },
                            new string[] {}
                    }
                };

    //ע�ᵽswagger��
    c.AddSecurityDefinition("bearerAuth", securityScheme);
    c.AddSecurityRequirement(securityRequirement);
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//�Ƿ���֤Issuer
        ValidateAudience = true,//�Ƿ���֤Audience
        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
        ClockSkew = TimeSpan.FromSeconds(60 * 60 * 24 * 7),
        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
        ValidAudience = JwtConfig.audience,//Audience
        ValidIssuer = JwtConfig.issuer,//Issuer���������ǰ��ǩ��jwt������һ��
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.securityKey))//�õ�SecurityKey
    };
});
//�����Ȩ����
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireClaim("IsAdmin","true","True","1"));
});

builder.Services.AddDbContext<bbsdbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//����CORS ��ʱ��������Ŀ
builder.Services.AddCors(options =>
{
    options.AddPolicy("any", builder =>
    {
        builder.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
        .AllowAnyHeader()
        //.AllowAnyMethod()
        .AllowAnyOrigin();
    });
});
//����

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    app.UseSwagger();
    app.UseSwaggerUI();

}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
//CORS
app.UseCors("any");

app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
