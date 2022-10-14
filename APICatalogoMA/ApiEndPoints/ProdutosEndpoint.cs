using APICatalogoMA.Context;
using APICatalogoMA.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogoMA.ApiEndPoints
{
    public static class ProdutosEndpoint
    {
        public static void MapProdutosEndpoints(this WebApplication app)
        {
            app.MapGet("/produtos", async (AppDbContext db) =>
                        await db.Produtos.ToListAsync()).WithTags("Produtos").RequireAuthorization();

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
        }
    }
}
