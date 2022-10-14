using APICatalogoMA.ApiEndPoints;
using APICatalogoMA.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //ConfigureServices


builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

//Ativar os servi�os de autoriza��o e autentica��o
app.UseAuthentication();
app.UseAuthorization();

app.Run();