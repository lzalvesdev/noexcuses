using NoExcusesFit.Data.Persistence;
using NoExcusesFit.Data.Repositories;
using NoExcusesFit.Domain.Interfaces.Repositories;
using NoExcusesFit.Worker;

var builder = Host.CreateApplicationBuilder(args);

//DI
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

var host = builder.Build();
host.Run();
