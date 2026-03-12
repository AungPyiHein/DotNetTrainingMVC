
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;


// STEP 1: Always create the builder FIRST
var builder = WebApplication.CreateBuilder(args);

// STEP 2: Now you can safely use builder.Configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt",
        rollingInterval: RollingInterval.Hour,
        fileSizeLimitBytes: 10485760,
        retainedFileCountLimit: 24)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DbConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        })
    .CreateLogger();

// STEP 3: Tell the app to use Serilog
builder.Host.UseSerilog();

// STEP 4: The rest of your services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Application Starting");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}


