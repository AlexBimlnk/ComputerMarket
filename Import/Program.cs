using Import;
using Import.Logic.Storage;

using Microsoft.EntityFrameworkCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddLogging();
builder.Services.AddDbContext<ImportContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetSection("ConnectionString").Value));
builder.Services.AddImportServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
