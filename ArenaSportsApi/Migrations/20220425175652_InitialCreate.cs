using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArenaSportsApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoUsuarioId = table.Column<int>(nullable: false),
                    CPF = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Churrasqueira",
                columns: table => new
                {
                    ChurrasqueiraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: true),
                    DescricaoItens = table.Column<string>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false),
                    UsuarioExclusaoId = table.Column<long>(nullable: true),
                    DataExclusao = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Churrasqueira", x => x.ChurrasqueiraId);
                    table.ForeignKey(
                        name: "FK_Churrasqueira_Usuario_UsuarioExclusaoId",
                        column: x => x.UsuarioExclusaoId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChurrasqueiraPacote",
                columns: table => new
                {
                    ChurrasqueiraPacoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: true),
                    Valor = table.Column<float>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    UsuarioExclusaoId = table.Column<long>(nullable: true),
                    DataExclusao = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurrasqueiraPacote", x => x.ChurrasqueiraPacoteId);
                    table.ForeignKey(
                        name: "FK_ChurrasqueiraPacote_Usuario_UsuarioExclusaoId",
                        column: x => x.UsuarioExclusaoId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quadra",
                columns: table => new
                {
                    QuadraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: true),
                    PisoId = table.Column<int>(nullable: false),
                    EsporteId = table.Column<int>(nullable: false),
                    Coberta = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    UsuarioExclusaoId = table.Column<long>(nullable: true),
                    DataExclusao = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quadra", x => x.QuadraId);
                    table.ForeignKey(
                        name: "FK_Quadra_Usuario_UsuarioExclusaoId",
                        column: x => x.UsuarioExclusaoId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuadraItemOpcional",
                columns: table => new
                {
                    QuadraItemOpcionalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: true),
                    Valor = table.Column<float>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    UsuarioExclusaoId = table.Column<long>(nullable: true),
                    DataExclusao = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuadraItemOpcional", x => x.QuadraItemOpcionalId);
                    table.ForeignKey(
                        name: "FK_QuadraItemOpcional_Usuario_UsuarioExclusaoId",
                        column: x => x.UsuarioExclusaoId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    ReservaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaUsuarioId = table.Column<long>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    DataReserva = table.Column<DateTime>(nullable: false),
                    ValorTotal = table.Column<float>(nullable: false),
                    DataRequisicao = table.Column<DateTime>(nullable: false),
                    RequisicaoUsuarioId = table.Column<long>(nullable: false),
                    CancelamentoUsuarioId = table.Column<long>(nullable: true),
                    DataCancelamento = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva", x => x.ReservaId);
                    table.ForeignKey(
                        name: "FK_Reserva_Usuario_CancelamentoUsuarioId",
                        column: x => x.CancelamentoUsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserva_Usuario_RequisicaoUsuarioId",
                        column: x => x.RequisicaoUsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserva_Usuario_ReservaUsuarioId",
                        column: x => x.ReservaUsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEmailConfirmacao",
                columns: table => new
                {
                    UsuarioEmailConfirmacaoId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    DataSolicitacao = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    DataConfirmacao = table.Column<DateTime>(nullable: true),
                    EmailConfirmado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEmailConfirmacao", x => x.UsuarioEmailConfirmacaoId);
                    table.ForeignKey(
                        name: "FK_UsuarioEmailConfirmacao_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuadraConfiguracaoHorario",
                columns: table => new
                {
                    QuadraConfiguracaoHorarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuadraId = table.Column<int>(nullable: false),
                    DiaSemana = table.Column<int>(nullable: false),
                    TimeInicio = table.Column<TimeSpan>(nullable: false),
                    TimeFim = table.Column<TimeSpan>(nullable: false),
                    Duracao = table.Column<TimeSpan>(nullable: false),
                    Valor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuadraConfiguracaoHorario", x => x.QuadraConfiguracaoHorarioId);
                    table.ForeignKey(
                        name: "FK_QuadraConfiguracaoHorario_Quadra_QuadraId",
                        column: x => x.QuadraId,
                        principalTable: "Quadra",
                        principalColumn: "QuadraId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservaChurrasqueira",
                columns: table => new
                {
                    ReservaChurrasqueiraId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaId = table.Column<long>(nullable: false),
                    ChurrasqueiraId = table.Column<int>(nullable: false),
                    ChurrasqueiraPacoteId = table.Column<int>(nullable: false),
                    Turno = table.Column<int>(nullable: false),
                    Valor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaChurrasqueira", x => x.ReservaChurrasqueiraId);
                    table.ForeignKey(
                        name: "FK_ReservaChurrasqueira_Churrasqueira_ChurrasqueiraId",
                        column: x => x.ChurrasqueiraId,
                        principalTable: "Churrasqueira",
                        principalColumn: "ChurrasqueiraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaChurrasqueira_ChurrasqueiraPacote_ChurrasqueiraPacoteId",
                        column: x => x.ChurrasqueiraPacoteId,
                        principalTable: "ChurrasqueiraPacote",
                        principalColumn: "ChurrasqueiraPacoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaChurrasqueira_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservaQuadra",
                columns: table => new
                {
                    ReservaQuadraId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaId = table.Column<long>(nullable: false),
                    QuadraId = table.Column<int>(nullable: false),
                    TimeInicio = table.Column<TimeSpan>(nullable: false),
                    TimeFim = table.Column<TimeSpan>(nullable: false),
                    Valor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaQuadra", x => x.ReservaQuadraId);
                    table.ForeignKey(
                        name: "FK_ReservaQuadra_Quadra_QuadraId",
                        column: x => x.QuadraId,
                        principalTable: "Quadra",
                        principalColumn: "QuadraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaQuadra_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservaQuadraOpcional",
                columns: table => new
                {
                    ReservaQuadraOpcionalId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaQuadraId = table.Column<long>(nullable: false),
                    QuadraItemOpcionalId = table.Column<int>(nullable: false),
                    Valor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaQuadraOpcional", x => x.ReservaQuadraOpcionalId);
                    table.ForeignKey(
                        name: "FK_ReservaQuadraOpcional_QuadraItemOpcional_QuadraItemOpcionalId",
                        column: x => x.QuadraItemOpcionalId,
                        principalTable: "QuadraItemOpcional",
                        principalColumn: "QuadraItemOpcionalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaQuadraOpcional_ReservaQuadra_ReservaQuadraId",
                        column: x => x.ReservaQuadraId,
                        principalTable: "ReservaQuadra",
                        principalColumn: "ReservaQuadraId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Churrasqueira_UsuarioExclusaoId",
                table: "Churrasqueira",
                column: "UsuarioExclusaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChurrasqueiraPacote_UsuarioExclusaoId",
                table: "ChurrasqueiraPacote",
                column: "UsuarioExclusaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Quadra_UsuarioExclusaoId",
                table: "Quadra",
                column: "UsuarioExclusaoId");

            migrationBuilder.CreateIndex(
                name: "IX_QuadraConfiguracaoHorario_QuadraId",
                table: "QuadraConfiguracaoHorario",
                column: "QuadraId");

            migrationBuilder.CreateIndex(
                name: "IX_QuadraItemOpcional_UsuarioExclusaoId",
                table: "QuadraItemOpcional",
                column: "UsuarioExclusaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_CancelamentoUsuarioId",
                table: "Reserva",
                column: "CancelamentoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_RequisicaoUsuarioId",
                table: "Reserva",
                column: "RequisicaoUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_ReservaUsuarioId",
                table: "Reserva",
                column: "ReservaUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaChurrasqueira_ChurrasqueiraId",
                table: "ReservaChurrasqueira",
                column: "ChurrasqueiraId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaChurrasqueira_ChurrasqueiraPacoteId",
                table: "ReservaChurrasqueira",
                column: "ChurrasqueiraPacoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaChurrasqueira_ReservaId",
                table: "ReservaChurrasqueira",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaQuadra_QuadraId",
                table: "ReservaQuadra",
                column: "QuadraId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaQuadra_ReservaId",
                table: "ReservaQuadra",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaQuadraOpcional_QuadraItemOpcionalId",
                table: "ReservaQuadraOpcional",
                column: "QuadraItemOpcionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaQuadraOpcional_ReservaQuadraId",
                table: "ReservaQuadraOpcional",
                column: "ReservaQuadraId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEmailConfirmacao_UsuarioId",
                table: "UsuarioEmailConfirmacao",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuadraConfiguracaoHorario");

            migrationBuilder.DropTable(
                name: "ReservaChurrasqueira");

            migrationBuilder.DropTable(
                name: "ReservaQuadraOpcional");

            migrationBuilder.DropTable(
                name: "UsuarioEmailConfirmacao");

            migrationBuilder.DropTable(
                name: "Churrasqueira");

            migrationBuilder.DropTable(
                name: "ChurrasqueiraPacote");

            migrationBuilder.DropTable(
                name: "QuadraItemOpcional");

            migrationBuilder.DropTable(
                name: "ReservaQuadra");

            migrationBuilder.DropTable(
                name: "Quadra");

            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
