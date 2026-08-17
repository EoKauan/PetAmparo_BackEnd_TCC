using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetAmparo.Domain.DTOs;
using PetAmparo.Domain.DTOs.Animal;
using PetAmparo.Domain.DTOs.Especie;
using PetAmparo.Domain.DTOs.Publicacao;
using PetAmparo.Domain.DTOs.Raca;
using PetAmparo.Domain.DTOs.Usuario;
using PetAmparo.Domain.Entities;
using PetAmparo.Domain.Enumerators;
using PetAmparo.Infra.Data.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using PetAmparo.Domain.AlterarSenha;
using PetAmparo.Domain.DTOs.Base;
using PetAmparo.Domain.Extensions;
using PetAmparo.Domain.ResetSenha;
using PetAmparo.Infra.Email;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<PetAmparoContext, PetAmparoContext>();

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PetAmparo",
        Version = "v1",
        Description = "API para adoção de animais"
    });

    // Configurar enums como strings no Swagger
    config.UseInlineDefinitionsForEnums();

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"<b>JWT Autorização</b> <br/>
                            Digite 'Bearer' [espaço] e em seguida cole seu token na caixa de texto abaixo.
                            <br/> <br/>
                            <b>Exemplo:</b> 'bearer 123456abcdefg...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "PetAmparo",
            ValidAudience = "PetAmparo",
            IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(
                  "{2d254698-de45-4f15-9b81-458d61b03793}"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddMapster();

// Configurar JSON para aceitar enums como strings
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(cors => cors
    .AllowAnyOrigin()
    .AllowAnyMethod() //GET - POST - PUT - DELETE
    .AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();

#region Autenticacao

app.MapPost("autenticar", (PetAmparoContext context, LoginDto loginDto) =>
{
    var resultado = new LoginDtoValidator().Validate(loginDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(p =>
        p.Email == loginDto.Email &&
        p.Senha == loginDto.Senha.EncryptPassword());

    if (usuario is null)
        return Results.BadRequest("Email ou Senha Inválidos");

    //Informações que desejamos guardar
    var claims = new[]
    {
        new Claim("Id", usuario.Id.ToString()),
        new Claim("Nome", usuario.Nome),
        new Claim("Login", usuario.Email),
        new Claim("Email", usuario.Email),
        new Claim("EhAdministrador", usuario.Administrador.ToString())
    };

    var key = new
        SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("{2d254698-de45-4f15-9b81-458d61b03793}"));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "PetAmparo",
        audience: "PetAmparo",
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: creds
    );

    var tokenGerado = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(tokenGerado);

}).WithTags("Autenticação");

app.MapPut("alterar-senha", (PetAmparoContext context, ClaimsPrincipal claims, AlterarSenhaDto alterarSenhaDto) =>
{
    var resultado = new AlterarSenhaDtoValidator().Validate(alterarSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var userId = Guid.Parse(userIdClaim);
    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Id == userId);
    if (usuario == null)
        return Results.NotFound("Usuário não encontrado.");

    if (usuario.Senha != alterarSenhaDto.Senha.EncryptPassword())
        return Results.BadRequest("Senha Atual não confere");

    usuario.Senha = alterarSenhaDto.NovaSenha.EncryptPassword();
    context.UsuarioSet.Update(usuario);
    context.SaveChanges();

    return Results.Ok("Senha alterada com sucesso.");
}).RequireAuthorization().WithTags("Segurança");

app.MapPost("gerar-chave-reset-senha", (PetAmparoContext context, GerarResetSenhaDto gerarResetSenhaDto) =>
{
    var resultado = new GerarResetSenhaDtoValidator().Validate(gerarResetSenhaDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Email == gerarResetSenhaDto.Email);

    if (usuario is not null)
    {
        usuario.ChaveResetSenha = Guid.NewGuid();
        context.UsuarioSet.Update(usuario);
        context.SaveChanges();

        var emailService = new EmailService();
        var enviarEmailResponse = emailService.EnviarEmail(gerarResetSenhaDto.Email, "Reset de Senha", $"https://petamparo.tccnapratica.com.br/reset-senha/{usuario.ChaveResetSenha}", true);
        if (!enviarEmailResponse.Sucesso)
            return Results.BadRequest($"Erro ao enviar o e-mail:{enviarEmailResponse.Mensagem}");
    }

    return Results.Ok("Se o e-mail informado estiver correto, você receberá as instruções por e-mail.");
}).WithTags("Segurança");

app.MapPut("resetar-senha", (PetAmparoContext context, ResetSenhaDto resetSenhaDto) =>
{
    var resultado = new ResetSenhaDtoValidator().Validate(resetSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(p => p.ChaveResetSenha == resetSenhaDto.ChaveResetSenha);

    if (usuario is null)
        return Results.BadRequest("Chave de reset de senha inválida.");

    usuario.Senha = resetSenhaDto.NovaSenha.EncryptPassword();
    usuario.ChaveResetSenha = null;
    context.UsuarioSet.Update(usuario);
    context.SaveChanges();

    return Results.Ok("Senha alterada com sucesso.");
}).WithTags("Segurança");

#endregion

#region Usuario

app.MapPost("usuario/adicionar", (PetAmparoContext context, UsuarioAdicionarDto usuarioAdicionarDto) =>
{
    var resultado = new UsuarioAdicionarDtoValidator().Validate(usuarioAdicionarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    usuarioAdicionarDto.Senha = usuarioAdicionarDto.Senha.EncryptPassword();

    context.UsuarioSet.Add(usuarioAdicionarDto.Adapt<Usuario>());
    context.SaveChanges();

    // Arrumar acima

    return Results.Created("Created", "Usuario Adicionado com Sucesso.");
}).WithTags("Usuário");

app.MapGet("usuario/listar", (PetAmparoContext context) =>
{

    var listaUsuario = context.UsuarioSet.ToList();
    return Results.Ok(listaUsuario.Adapt<List<UsuarioListarDto>>());

    //Forma manual de mapear DTO -> Entidade
    //var listaUsuarioDto = listaUsuario.Select(u => new UsuarioListarDto
    //{
    //    Id = u.Id,
    //    Email = u.Email,
    //    Foto = u.Foto,
    //    Municipio = u.Municipio,
    //    Nome = u.Nome,
    //    Telefone = u.Telefone
    //});

    //return Results.Ok(listaUsuarioDto);

}).WithTags("Usuário");

app.MapGet("usuario/obter/{id:guid}", (PetAmparoContext context, Guid id) =>
{

    var usuario = context.UsuarioSet.Find(id);
    if (usuario is null)
        return Results.BadRequest("Usuário não Localizado.");

    return Results.Ok(usuario.Adapt<UsuarioObterDto>());
}).WithTags("Usuário");

app.MapPut("usuario/atualizar", (PetAmparoContext context, UsuarioAtualizarDto usuarioAtualizarDto) =>
{
    var resultado = new UsuarioAtualizarDtoValidator().Validate(usuarioAtualizarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.Find(usuarioAtualizarDto.Id);
    if (usuario is null)
        return Results.BadRequest("Usuário não Localizado.");

    // Atualizar apenas os campos que foram informados
    if (!string.IsNullOrEmpty(usuarioAtualizarDto.Nome))
        usuario.Nome = usuarioAtualizarDto.Nome;

    if (!string.IsNullOrEmpty(usuarioAtualizarDto.Email))
        usuario.Email = usuarioAtualizarDto.Email;

    if (!string.IsNullOrEmpty(usuarioAtualizarDto.Telefone))
        usuario.Telefone = usuarioAtualizarDto.Telefone;

    if (!string.IsNullOrEmpty(usuarioAtualizarDto.Municipio))
        usuario.Municipio = usuarioAtualizarDto.Municipio;

    if (!string.IsNullOrEmpty(usuarioAtualizarDto.Foto))
        usuario.Foto = usuarioAtualizarDto.Foto;

    if (usuarioAtualizarDto.Bio != null)
        usuario.Bio = usuarioAtualizarDto.Bio;

    if (usuarioAtualizarDto.Administrador.HasValue)
        usuario.Administrador = usuarioAtualizarDto.Administrador.Value;

    // Atualizar senha apenas se fornecida
    if (!string.IsNullOrEmpty(usuarioAtualizarDto.Senha))
    {
        usuario.Senha = usuarioAtualizarDto.Senha.EncryptPassword();
    }

    context.UsuarioSet.Update(usuario);
    context.SaveChanges();

    return Results.Ok("Usuario Atualizado com Sucesso.");
}).WithTags("Usuário");

app.MapDelete("usuario/excluir/{id:guid}", (PetAmparoContext context, Guid id) =>
{ 

    var usuario = context.UsuarioSet.Find(id);

    if (usuario is null)
        return Results.BadRequest("Usuario não encontrado.");

    // Remover todos os likes do usuário antes de deletar
    var likesDoUsuario = context.PublicacaoLikeSet.Where(l => l.UsuarioId == id).ToList();
    if (likesDoUsuario.Any())
    {
        context.PublicacaoLikeSet.RemoveRange(likesDoUsuario);
    }

    context.UsuarioSet.Remove(usuario);
    context.SaveChanges();

    return Results.Ok("Usuário Excluído com Sucesso.");
}).WithTags("Usuário");

#endregion 

#region Animal

app.MapPost("animal/adicionar", (PetAmparoContext context, ClaimsPrincipal claims, AnimalAdicionarDto animalAdicionarDto) =>
{
    var resultado = new AnimalAdicionarDtoValidator().Validate(animalAdicionarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var userId = Guid.Parse(userIdClaim);
    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Id == userId);
    if (usuario == null)
        return Results.NotFound("Usuário não encontrado.");

    var animal = animalAdicionarDto.Adapt<Animal>();
    animal.Status = EnumStatusAnimal.Disponivel;
    animal.UsuarioId = usuario.Id;

    context.AnimalSet.Add(animal);
    context.SaveChanges();

    // Adicionar fotos
    foreach (var fotoUrl in animalAdicionarDto.Fotos)
    {
        var animalFoto = new AnimalFoto
        {
            Id = Guid.NewGuid(),
            AnimalId = animal.Id,
            Foto = fotoUrl
        };
        context.AnimalFotoSet.Add(animalFoto);
    }
    context.SaveChanges();

    return Results.Created("Created", "Animal Adicionado com Sucesso.");
}).RequireAuthorization().WithTags("Animal");

app.MapGet("animal/listar", (PetAmparoContext context) =>
{
    var listaAnimal = context.AnimalSet.Where(p => p.Status == EnumStatusAnimal.Disponivel)
        .Include(p => p.AnimalFoto)
        .Include(p => p.Raca)
        .Include(p => p.Usuario)
        .Include(p => p.Especie)
        .ToList();
    
    var listaAnimalDto = listaAnimal.Adapt<List<AnimalListarDto>>();
    
    // Mapear fotos para cada animal
    for (int i = 0; i < listaAnimal.Count; i++)
    {
        listaAnimalDto[i].Fotos = listaAnimal[i].AnimalFoto.Select(f => f.Foto).ToList();
    }

    for (int i = 0; i < listaAnimal.Count; i++)
    {
        listaAnimalDto[i].Raca = listaAnimal[i].Raca.Adapt<RacaListarDto>();
    }

    for (int i = 0; i < listaAnimal.Count; i++)
    {
        listaAnimalDto[i].Usuario = listaAnimal[i].Usuario.Adapt<UsuarioListarDto>();
    }

    return Results.Ok(listaAnimalDto);
}).WithTags("Animal");

app.MapGet("animal/obter/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var animal = context.AnimalSet
        .AsNoTracking()
        .Include(p => p.AnimalFoto)
        .Include(p => p.Raca)
        .Include(p => p.Usuario)
        .Include(p => p.Especie)
        .SingleOrDefault(p => p.Id == id);

    if (animal is null)
        return Results.BadRequest("Animal não Localizado.");

    var animalDto = animal.Adapt<AnimalObterDto>();
    animalDto.Fotos = animal.AnimalFoto.Select(f => f.Foto).ToList();

    animalDto.Raca = animalDto.Raca.Adapt<RacaListarDto>();

    animalDto.Usuario = animal.Usuario.Adapt<UsuarioListarDto>();
    

    return Results.Ok(animalDto);
}).WithTags("Animal");

app.MapGet("animal/listar-por-especie/{especieId:guid}", (PetAmparoContext context, Guid especieId) =>
{
    var animais = context.AnimalSet
        .Include(p => p.AnimalFoto)
        .Where(r => r.EspecieId == especieId)
        .ToList();
    
    var animaisDto = animais.Adapt<List<AnimalListarDto>>();
    
    // Mapear fotos para cada animal
    for (int i = 0; i < animais.Count; i++)
    {
        animaisDto[i].Fotos = animais[i].AnimalFoto.Select(f => f.Foto).ToList();
    }

    if (!animaisDto.Any())
        return Results.BadRequest(new BaseResponse("Não há animais para serem listados"));

    return Results.Ok(animaisDto);
}).WithTags("Animal");

app.MapGet("animal/listar-por-usuario", (PetAmparoContext context, ClaimsPrincipal claims) =>
{
    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var listaAnimal = context.AnimalSet
        .Where(a => a.UsuarioId == userId)
        .Include(a => a.AnimalFoto)
        .Include(a => a.Raca)
        .ThenInclude(r => r.Especie)
        .Include(a => a.Usuario)
        .Include(a => a.Especie)
        .ToList();

    var listaAnimalDto = listaAnimal.Select(la => new AnimalListarDto
    {
        Id = la.Id,
        Nome = la.Nome,
        Idade = la.Idade,
        Observacao = la.Observacao,
        Status = la.Status,
        EspecieId = la.EspecieId,
        Especie = new EspecieListarDto
        {
            Id = la.Especie.Id,
            Descricao = la.Especie.Descricao
        },
        UsuarioId = la.UsuarioId,
        Usuario = new UsuarioListarDto
        {
            Id = la.Usuario.Id,
            Nome = la.Usuario.Nome,
            Email = la.Usuario.Email,
            Telefone = la.Usuario.Telefone,
            Municipio = la.Usuario.Municipio,
            Foto = la.Usuario.Foto,
            Bio = la.Usuario.Bio,
            Administrador = la.Usuario.Administrador
        },
        RacaId = la.RacaId,
        Raca = new RacaListarDto
        {
            Id = la.Raca.Id,
            Descricao = la.Raca.Descricao,
            Especie = new EspecieListarDto
            {
                Id = la.Raca.Especie.Id,
                Descricao = la.Raca.Especie.Descricao
            }
        },
        Fotos = la.AnimalFoto.Select(p => p.Foto).ToList()
    });

    return Results.Ok(listaAnimalDto);
}).RequireAuthorization().WithTags("Animal");

app.MapPut("animal/atualizar", (PetAmparoContext context, AnimalAtualizarDto animalAtualizarDto) =>
{
    var resultado = new AnimalAtualizarDtoValidator().Validate(animalAtualizarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var animal = context.AnimalSet
        .Include(p => p.AnimalFoto)
        .FirstOrDefault(p => p.Id == animalAtualizarDto.Id);

    if (animal is null)
        return Results.BadRequest("Animal não Localizado.");

    // Remover fotos antigas
    var fotosAntigas = context.AnimalFotoSet.Where(f => f.AnimalId == animalAtualizarDto.Id).ToList();
    context.AnimalFotoSet.RemoveRange(fotosAntigas);

    // Atualizar dados do animal
    animal.Nome = animalAtualizarDto.Nome;
    animal.EspecieId = animalAtualizarDto.EspecieId;
    animal.RacaId = animalAtualizarDto.RacaId;
    animal.Idade = animalAtualizarDto.Idade;
    animal.Observacao = animalAtualizarDto.Observacao;
    animal.Status = animalAtualizarDto.Status;
    
    context.AnimalSet.Update(animal);
    context.SaveChanges();

    // Adicionar novas fotos
    foreach (var fotoUrl in animalAtualizarDto.Fotos)
    {
        var animalFoto = new AnimalFoto
        {
            Id = Guid.NewGuid(),
            AnimalId = animal.Id,
            Foto = fotoUrl
        };
        context.AnimalFotoSet.Add(animalFoto);
    }
    context.SaveChanges();

    return Results.Ok("Animal Atualizado com Sucesso.");
}).RequireAuthorization().WithTags("Animal");

app.MapDelete("animal/excluir/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var animal = context.AnimalSet
        .Include(p => p.AnimalFoto)
        .FirstOrDefault(p => p.Id == id);

    if (animal is null)
        return Results.BadRequest("Animal não encontrado.");

    // Remover fotos associadas
    if (animal.AnimalFoto.Any())
    {
        context.AnimalFotoSet.RemoveRange(animal.AnimalFoto);
    }

    context.AnimalSet.Remove(animal);
    context.SaveChanges();

    return Results.Ok("Animal Excluído com Sucesso.");
}).RequireAuthorization().WithTags("Animal");

#endregion

#region publicacao

app.MapPost("publicacao/adicionar", (PetAmparoContext context, ClaimsPrincipal claims, PublicacaoAdicionarDto publicacaoAdicionarDto) =>
{
    var resultado = new PublicacaoAdicionarDtoValidator().Validate(publicacaoAdicionarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var userId = Guid.Parse(userIdClaim);
    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Id == userId);
    if (usuario == null)
        return Results.NotFound("Usuário não encontrado.");

    var publicacao = new Publicacao
    {
        Id = Guid.NewGuid(),
        Titulo = publicacaoAdicionarDto.Titulo,
        Foto = publicacaoAdicionarDto.Foto,
        Data = DateTime.Now,
        UsuarioId = usuario.Id
    };
    
    context.PublicacaoSet.Add(publicacao);
    context.SaveChanges();

    return Results.Created("Created", "Publicação Adicionado com Sucesso.");
}).RequireAuthorization().WithTags("Publicação");

app.MapGet("publicacao/listar", (PetAmparoContext context) =>
{
    var listaPublicacao = context.PublicacaoSet
        .Include(p => p.Usuario)
        .ToList();
    
    var listaPublicacaoDto = listaPublicacao.Adapt<List<PublicacaoListarDto>>();
    return Results.Ok(listaPublicacaoDto);
}).WithTags("Publicação");

app.MapGet("publicacao/listar-por-usuario", (PetAmparoContext context, ClaimsPrincipal claims) =>
{
    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var usuarioId = Guid.Parse(userIdClaim);

    var listaPublicacao = context.PublicacaoSet
        .Include(p => p.Usuario)
        .Where(p => p.UsuarioId == usuarioId)
        .ToList();

    var listaPublicacaoDto = listaPublicacao.Adapt<List<PublicacaoListarDto>>();
    return Results.Ok(listaPublicacaoDto);
}).RequireAuthorization().WithTags("Publicação");

app.MapGet("publicacao/obter/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var publicacao = context.PublicacaoSet
        .AsNoTracking()
        .Include(p => p.Usuario)
        .SingleOrDefault(p => p.Id == id);
    
    if (publicacao is null)
        return Results.BadRequest("Publicação não Localizado.");

    var publicacaoDto = publicacao.Adapt<PublicacaoObterDto>();

    return Results.Ok(publicacaoDto);
}).WithTags("Publicação");

app.MapPut("publicacao/atualizar", (PetAmparoContext context, ClaimsPrincipal claims, PublicacaoAtualizarDto publicacaoAtualizarDto) =>
{
    var resultado = new PublicacaoAtualizarDtoValidator().Validate(publicacaoAtualizarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    // Obter ID do usuário autenticado
    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var publicacao = context.PublicacaoSet
        .FirstOrDefault(p => p.Id == publicacaoAtualizarDto.Id);

    if (publicacao is null)
        return Results.BadRequest("Publicação não Localizada.");

    // Atualizar apenas os campos que foram informados
    if (!string.IsNullOrEmpty(publicacaoAtualizarDto.Titulo))
        publicacao.Titulo = publicacaoAtualizarDto.Titulo;

    if (publicacaoAtualizarDto.Data.HasValue)
        publicacao.Data = publicacaoAtualizarDto.Data.Value;

    if (publicacaoAtualizarDto.Foto != null)
        publicacao.Foto = publicacaoAtualizarDto.Foto;

    if (publicacaoAtualizarDto.UsuarioId.HasValue)
        publicacao.UsuarioId = publicacaoAtualizarDto.UsuarioId.Value;

    // Gerenciar like se foi informado
    if (publicacaoAtualizarDto.Like.HasValue)
    {
        var likeExistente = context.PublicacaoLikeSet
            .FirstOrDefault(l => l.PublicacaoId == publicacao.Id && l.UsuarioId == userId);

        if (publicacaoAtualizarDto.Like.Value)
        {
            // Adicionar like se não existir
            if (likeExistente == null)
            {
                var novoLike = new PublicacaoLike
                {
                    Id = Guid.NewGuid(),
                    PublicacaoId = publicacao.Id,
                    UsuarioId = userId
                };
                context.PublicacaoLikeSet.Add(novoLike);
            }
        }
        else
        {
            // Remover like se existir
            if (likeExistente != null)
            {
                context.PublicacaoLikeSet.Remove(likeExistente);
            }
        }
    }

    context.PublicacaoSet.Update(publicacao);
    context.SaveChanges();

    return Results.Ok("Publicação Atualizada com Sucesso.");
}).RequireAuthorization().WithTags("Publicação");

app.MapDelete("publicacao/excluir/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var publicacao = context.PublicacaoSet
        .FirstOrDefault(p => p.Id == id);

    if (publicacao is null)
        return Results.BadRequest("Publicação não encontrado.");

    context.PublicacaoSet.Remove(publicacao);
    context.SaveChanges();

    return Results.Ok("Publicação Excluído com Sucesso.");
}).RequireAuthorization().WithTags("Publicação");

#endregion

#region Raca

app.MapPost("raca/adicionar", (PetAmparoContext context, RacaAdicionarDto racaAdicionarDto) =>
{
    var resultado = new RacaAdicionarDtoValidator().Validate(racaAdicionarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    context.RacaSet.Add(racaAdicionarDto.Adapt<Raca>());
    context.SaveChanges();

    return Results.Created("Created", "Raca do Animal Adicionado com Sucesso.");
}).RequireAuthorization().WithTags("Raça");

app.MapGet("raca/listar", (PetAmparoContext context) =>
{
    var listarRaca = context.RacaSet
        .Include(p => p.Especie)
        .ToList();

    var listarRacaDto = listarRaca.Adapt<List<RacaListarDto>>();

    return Results.Ok(listarRacaDto);
}).WithTags("Raça");

app.MapGet("raca/obter/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var raca = context.RacaSet
        .Include(p => p.Especie)
        .FirstOrDefault(p => p.Id == id);
    
    if (raca is null)
        return Results.BadRequest("Raça não Localizado.");

    var racaDto = raca.Adapt<RacaObterDto>();
    racaDto.Especie = raca.Especie.Adapt<EspecieObterDto>();

    return Results.Ok(racaDto);
}).WithTags("Raça");

app.MapGet("raca/listar-por-especie/{especieId:guid}", (PetAmparoContext context, Guid especieId) =>
{
    var racas = context.RacaSet
        .Include(p => p.Especie)
        .Where(r => r.EspecieId == especieId)
        .ToList();
    
    var racasDto = racas.Adapt<List<RacaListarDto>>();

    return Results.Ok(racasDto);
}).WithTags("Raça");

app.MapPut("raca/atualizar", (PetAmparoContext context, RacaAtualizarDto racaAtualizarDto) =>
{
    var resultado = new RacaAtualizarDtoValidator().Validate(racaAtualizarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var raca = context.RacaSet.Find(racaAtualizarDto.Id);
    if (raca is null)
        return Results.BadRequest("Raça não Localizado.");

    context.Entry(raca).State = EntityState.Detached;

    context.RacaSet.Update(racaAtualizarDto.Adapt<Raca>());
    context.SaveChanges();

    return Results.Ok("Raça Atualizado com Sucesso.");
}).RequireAuthorization().WithTags("Raça");

app.MapDelete("raca/excluir/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var raca = context.RacaSet.Find(id);

    if (raca is null)
        return Results.BadRequest("Raça não encontrado.");

    context.RacaSet.Remove(raca);
    context.SaveChanges();

    return Results.Ok("Raça Excluído com Sucesso.");
}).RequireAuthorization().WithTags("Raça");
#endregion

#region Especie

app.MapPost("especie/adicionar", (PetAmparoContext context, EspecieAdicionarDto especieAdicionarDto) =>
{
    var resultado = new EspecieAdicionarDtoValidator().Validate(especieAdicionarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    context.EspecieSet.Add(especieAdicionarDto.Adapt<Especie>());
    context.SaveChanges();

    return Results.Created("Created", "Espécie Adicionada com Sucesso.");
}).RequireAuthorization().WithTags("Espécie");

app.MapGet("especie/listar", (PetAmparoContext context) =>
{
    var listarEspecie = context.EspecieSet.ToList();

    return Results.Ok(listarEspecie.Adapt<List<EspecieListarDto>>());
}).WithTags("Espécie");

app.MapGet("especie/obter/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var especie = context.EspecieSet.Find(id);
    if (especie is null)
        return Results.BadRequest("Espécie não Localizada.");

    return Results.Ok(especie.Adapt<EspecieObterDto>());
}).WithTags("Espécie");

app.MapPut("especie/atualizar", (PetAmparoContext context, EspecieAtualizarDto especieAtualizarDto) =>
{
    var resultado = new EspecieAtualizarDtoValidator().Validate(especieAtualizarDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var especie = context.EspecieSet.Find(especieAtualizarDto.Id);
    if (especie is null)
        return Results.BadRequest("Espécie não Localizada.");

    context.Entry(especie).State = EntityState.Detached;

    context.EspecieSet.Update(especieAtualizarDto.Adapt<Especie>());
    context.SaveChanges();

    return Results.Ok("Espécie Atualizada com Sucesso.");
}).RequireAuthorization().WithTags("Espécie");

app.MapDelete("especie/excluir/{id:guid}", (PetAmparoContext context, Guid id) =>
{
    var especie = context.EspecieSet.Find(id);

    if (especie is null)
        return Results.BadRequest("Espécie não encontrada.");

    // Remover todas as raças associadas antes de deletar a espécie
    var racasDaEspecie = context.RacaSet.Where(r => r.EspecieId == id).ToList();
    if (racasDaEspecie.Any())
    {
        // Verificar se há animais usando essas raças
        foreach (var raca in racasDaEspecie)
        {
            var animaisComRaca = context.AnimalSet.Where(a => a.RacaId == raca.Id).ToList();
            if (animaisComRaca.Any())
            {
                return Results.BadRequest($"Não é possível excluir a espécie pois existem animais cadastrados com raças desta espécie.");
            }
        }
        
        // Se não houver animais, remover as raças
        context.RacaSet.RemoveRange(racasDaEspecie);
    }

    context.EspecieSet.Remove(especie);
    context.SaveChanges();

    return Results.Ok("Espécie Excluída com Sucesso.");
}).RequireAuthorization().WithTags("Espécie");

#endregion

app.Run();
