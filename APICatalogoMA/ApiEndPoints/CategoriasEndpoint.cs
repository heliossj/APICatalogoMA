using APICatalogoMA.Context;
using APICatalogoMA.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogoMA.ApiEndPoints
{
    public static class CategoriasEndpoint
    {
        public static void MapCategoriasEndpoints(this WebApplication app)
        {
            app.MapGet("/categorias", async (AppDbContext db) =>
                          await db.Categorias.ToListAsync()).WithTags("Categorias").RequireAuthorization();

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
        }
    }
}
