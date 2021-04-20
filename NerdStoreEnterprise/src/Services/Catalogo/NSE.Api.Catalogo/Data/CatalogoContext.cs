using Microsoft.EntityFrameworkCore;
using NSE.Api.Catalogo.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Api.Catalogo.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> context) : base(context)
        {

        }

        // Usado pois fizemos os mapeamentos - migrations
        public DbSet<Produto> Produtos { get; set; }

        // Mapping - usado por causa do IEntityTypeConfiguration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Causa alguma propriedade nao esta mapeda
            // ele seta o tipo string como varchar(100)
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(c => c.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }

        // Commit - Salvar
        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
