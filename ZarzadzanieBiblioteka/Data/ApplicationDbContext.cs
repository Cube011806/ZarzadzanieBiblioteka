﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ZarzadzanieBiblioteka.Models;

namespace ZarzadzanieBiblioteka.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
		public virtual DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public virtual DbSet<Autor> Autorzy { get; set; }
        public virtual DbSet<Biblioteka> Biblioteki { get; set; }
        public virtual DbSet<Ksiazka> Ksiazki { get; set; }
        public virtual DbSet<Opinia> Opinie { get; set; }
        public virtual DbSet<Wypozyczenie> Wypozyczenia { get; set; }
        public virtual DbSet<Rezerwacja> Rezerwacje { get; set; }
        public virtual DbSet<Wolumin> Woluminy { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Ksiazka>()
                .HasOne(k => k.Autor)
                .WithMany(a => a.Ksiazki)
                .HasForeignKey(k => k.AutorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}