using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetAmparo.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Especie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Descricao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Especie", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Municipio = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bio = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Administrador = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ChaveResetSenha = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Usuario", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Raca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Descricao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EspecieId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Raca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Raca_TB_Especie_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "TB_Especie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Publicacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Publicacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Publicacao_TB_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Animal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idade = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EspecieId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RacaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Animal_TB_Especie_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "TB_Especie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Animal_TB_Raca_RacaId",
                        column: x => x.RacaId,
                        principalTable: "TB_Raca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Animal_TB_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_PublicacaoLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PublicacaoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PublicacaoLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_PublicacaoLike_TB_Publicacao_PublicacaoId",
                        column: x => x.PublicacaoId,
                        principalTable: "TB_Publicacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_PublicacaoLike_TB_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_AnimalFoto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnimalId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Foto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AnimalFoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_AnimalFoto_TB_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "TB_Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Animal_EspecieId",
                table: "TB_Animal",
                column: "EspecieId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Animal_RacaId",
                table: "TB_Animal",
                column: "RacaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Animal_UsuarioId",
                table: "TB_Animal",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AnimalFoto_AnimalId",
                table: "TB_AnimalFoto",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Publicacao_UsuarioId",
                table: "TB_Publicacao",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PublicacaoLike_PublicacaoId_UsuarioId",
                table: "TB_PublicacaoLike",
                columns: new[] { "PublicacaoId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_PublicacaoLike_UsuarioId",
                table: "TB_PublicacaoLike",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Raca_EspecieId",
                table: "TB_Raca",
                column: "EspecieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AnimalFoto");

            migrationBuilder.DropTable(
                name: "TB_PublicacaoLike");

            migrationBuilder.DropTable(
                name: "TB_Animal");

            migrationBuilder.DropTable(
                name: "TB_Publicacao");

            migrationBuilder.DropTable(
                name: "TB_Raca");

            migrationBuilder.DropTable(
                name: "TB_Usuario");

            migrationBuilder.DropTable(
                name: "TB_Especie");
        }
    }
}
