using APICatalogoMA.Context;
using APICatalogoMA.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //ConfigureServices
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var app = builder.Build();

//app.MapGet("/", () => "Catálogo de produtos - 2022");

//========================== Endpoints para Categorias ==========================
app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    //var cat = await db.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);
    //if (cat is null)
    //    return Results.NotFound();
    //return Results.Ok(cat);
    return await db.Categorias.FindAsync(id) is Categoria categoria ? Results.Ok(categoria) : Results.NotFound();
});

app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
{
    if (categoria.CategoriaId != id)
        return Results.BadRequest();

    var categoriaDB = await db.Categorias.FindAsync(id);
    if (categoriaDB is null)
        return Results.NotFound();

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;
    db.Categorias.Update(categoriaDB);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var cat = await db.Categorias.FindAsync(id);
    if (cat is null)
        return Results.NotFound();
    db.Categorias.Remove(cat);
    await db.SaveChangesAsync();
    return Results.Ok(cat);
});

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

//========================== Endpoints para Categorias ==========================

app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync());

app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound();
});

app.MapPost("produtos", async (Produto produto, AppDbContext db) =>
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
});

app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
{
    var produtoDB = await db.Produtos.FindAsync(id);

    if (produtoDB.CategoriaId != id)
        return Results.NotFound();

    produtoDB.Nome = produto.Nome;
    produtoDB.Descricao = produto.Descricao;
    produtoDB.Preco = produto.Preco;
    produtoDB.Imagem = produto.Imagem;
    produtoDB.DataCompra = produto.DataCompra;
    produtoDB.Estoque = produto.Estoque;
    produtoDB.CategoriaId = produto.CategoriaId;

    db.Produtos.Update(produtoDB);
    await db.SaveChangesAsync();

    return Results.Ok(produtoDB);
});

app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
{
    var prod = await db.Produtos.FindAsync(id);

    if (prod is null)
        return Results.NotFound();
    db.Produtos.Remove(prod);
    await db.SaveChangesAsync();
    return Results.Ok(prod);
});



// Configure the HTTP request pipeline. //Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();