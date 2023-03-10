var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.Use(async (context, next) =>
{
    var maxRequestBodySize = 104857600; // 100 MB
    if (context.Request.ContentLength > maxRequestBodySize)
    {
        context.Response.StatusCode = 413; // Payload Too Large
        await context.Response.WriteAsync("Payload Too Large");
        return;
    }

    await next();
});

app.Run();
