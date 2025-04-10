using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using VSRAdminAPI.Services;
using VSRAdminAPI.Model;
using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Repository;
using Microsoft.AspNetCore.Antiforgery;
using static System.Net.Mime.MediaTypeNames;
using VSRAdminAPI.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(80);
    });
}

// Add configuration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "VSRAdmin API", 
        Version = "v1",
        Description = "API for VSR Admin operations"
    });
    
    // Add security definition if you have authentication
    // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {...});
});

builder.Services.AddLogging();

// Register services
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRestaurantInstructionService, RestaurantInstructionService>();
builder.Services.AddScoped<IRestaurantInstructions, RestaurantInstructions>();

var app = builder.Build();

// Middleware pipeline
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable Swagger based on configuration or environment
var swaggerEnabled = app.Configuration.GetValue<bool>("SWAGGER_ENABLED", false);
if (app.Environment.IsDevelopment() || swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VSRAdmin API v1");
        c.RoutePrefix = "swagger";  // Access at /swagger
    });
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

// Endpoints
app.MapPost("/api/ValidateLogin", ([FromBody] LoginValues loginvalues, [FromServices] ICompanyService companyService) =>
{
    var genericResponse = companyService.ValidateLogin(loginvalues);
    return Results.Ok(genericResponse);
}).WithTags("Login");

app.MapPost("/api/Restaurant", async (HttpRequest request,
    [FromServices] ICompanyService companyService, [FromServices] ILogger<Program> logger) =>
{
    try
    {
        var form = await request.ReadFormAsync();
        var customerdata = form["customerdata"].FirstOrDefault();
        var file = form.Files.FirstOrDefault();

        MasterCustomer objContact;
        try
        {
            objContact = JsonConvert.DeserializeObject<MasterCustomer>(customerdata);
            if (objContact == null)
            {
                throw new InvalidOperationException("Deserialization returned null.");
            }
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Invalid JSON format in customerdata: {Data}", customerdata);
            return Results.BadRequest(new { Message = "Invalid customer data format." });
        }

        var customerFileData = new CustomerFileData
        {
            Customerdata = customerdata,
            Filename = file
        };
        
        var directory = Path.Combine("D:\\var\\www\\restaurantlogo");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        if (file != null)
        {
            var filePath = Path.Combine(directory, $"{objContact.DID}.jpg");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        GenericResponse response = companyService.AddCompany(customerFileData);
        logger.LogInformation("AddCompany executed successfully for customerdata: {Data}", customerdata);
        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unhandled error in AddCompany: {Message}", ex.Message);
        return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
    }
})
.WithTags("Restaurant")
.Accepts<CustomerFileData>("multipart/form-data")
.Produces<GenericResponse>(200);

app.MapGet("/api/Restaurant", ([FromQuery] string? search, [FromQuery] int pageno, 
    [FromServices] ICompanyService companyService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Received Search: {Search}, Page Number: {Pageno}", search, pageno);

        var companySearch = new CompanySearch
        {
            Search = string.IsNullOrWhiteSpace(search) ? string.Empty : search,
            Pageno = pageno
        };

        var genericResponse = companyService.LoadCompany(companySearch);

        if (genericResponse.Data is IDictionary<string, object> responseData && responseData.ContainsKey("totalrow"))
        {
            var totalRow = (int)responseData["totalrow"];
            logger.LogInformation("Database returned {TotalRowCount} rows", totalRow);

            if (totalRow == 0)
            {
                logger.LogWarning("No data found for Search: {Search}, Page: {Pageno}", search, pageno);
            }

            return Results.Ok(genericResponse);
        }
        else
        {
            logger.LogWarning("Response data does not contain 'totalrow' for Search: {Search}, Page: {Pageno}", search, pageno);
            return Results.Ok(genericResponse);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred in LoadCompany: {ErrorMessage}", ex.Message);
        return Results.Problem($"An error occurred: {ex.Message}", statusCode: 500);
    }
}).WithTags("Restaurant");

app.MapPost("/api/Instruction", ([FromBody] ReqInput reqInput, 
    [FromServices] IRestaurantInstructionService restaurantInstructionService) =>
{
    var genericResponse = restaurantInstructionService.AddInstruction(reqInput);
    return Results.Ok(genericResponse);
}).WithTags("Restaurant");

app.MapGet("/api/Instruction", (int customerid, 
    [FromServices] IRestaurantInstructionService restaurantInstructionService) =>
{
    var genericResponse = restaurantInstructionService.LoadInstruction(customerid);
    return Results.Ok(genericResponse);
}).WithTags("Restaurant");

app.MapPost("/api/CustomerInfo", ([FromBody] CustomerInfo addcustomerinfo, 
    [FromServices] ICustomerService customerService, [FromServices] ILogger<Program> logger) =>
{
    try
    {
        GenericResponse genericResponse = customerService.Customerinfo(addcustomerinfo);
        return Results.Ok(genericResponse);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred in AddCustomerInfo: {ErrorMessage}", ex.Message);
        return Results.Problem($"An error occurred: {ex.Message}", statusCode: 500);
    }
}).WithTags("CustomerInfo");

// Root and Health endpoints
app.MapGet("/", () => Results.Ok("VSRAdmin API is running ✅"));
app.MapGet("/health", () => Results.Ok("Healthy ✅"));

app.Run();
