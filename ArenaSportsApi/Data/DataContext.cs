using ArenaSportsApi.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<UsuarioEmailConfirmacao> UsuarioEmailConfirmacao { get; set; }
    public DbSet<Quadra> Quadra { get; set; }
    public DbSet<QuadraConfiguracaoHorario> QuadraConfiguracaoHorario { get; set; }
    public DbSet<Churrasqueira> Churrasqueira { get; set; }
    public DbSet<Reserva> Reserva { get; set; }
    public DbSet<ReservaQuadra> ReservaQuadra { get; set; }
    public DbSet<ReservaQuadraOpcional> ReservaQuadraOpcional { get; set; }
    public DbSet<QuadraItemOpcional> QuadraItemOpcional { get; set; }
    public DbSet<ReservaChurrasqueira> ReservaChurrasqueira { get; set; }
    public DbSet<ChurrasqueiraPacote> ChurrasqueiraPacote { get; set; }
}