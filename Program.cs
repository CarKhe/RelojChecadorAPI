using relojChecadorAPI;
using relojChecadorAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DbRelojChecadorContext>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IUsuarioAreaService, UsuarioAreaService>();
builder.Services.AddScoped<IAsistenciaService,AsistenciaService>();
builder.Services.AddScoped<IFkCheck, FkCheck>();
builder.Services.AddScoped<ISyntaxisDB, SyntaxisDB>();
builder.Services.AddScoped<IMensajesDB,MensajesDB>();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
