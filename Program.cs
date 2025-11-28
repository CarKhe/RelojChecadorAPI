using System.Text;
using relojChecadorAPI;
using relojChecadorAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// ---- JWT ---- //
var jwtConfig = builder.Configuration.GetSection("Jwt");
var keyString = jwtConfig["Key"] ?? throw new Exception("JWT Key not found in configuration.");
var key = Encoding.UTF8.GetBytes(keyString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ---- JWT ---- //

builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DbRelojChecadorContext>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IUsuarioAreaService, UsuarioAreaService>();
builder.Services.AddScoped<IAsistenciaService,AsistenciaService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IFkCheck, FkCheck>();
builder.Services.AddScoped<ISyntaxisDB, SyntaxisDB>();
builder.Services.AddScoped<IMensajesDB,MensajesDB>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddAutoMapper(typeof(Program));

//Politicas CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//USO DE LAS CORS
app.UseCors("AngularPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
