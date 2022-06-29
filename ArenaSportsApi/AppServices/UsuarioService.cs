using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

public class UsuarioService : IUsuarioService
{
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepo;
    private readonly IConfiguration _configuration;
    private readonly Regex _regexSenha;
    private readonly IUsuarioRepository _userRepo;

    public UsuarioService(IMapper mapper, DataContext context, IAuthRepository authRepository, IConfiguration configuration, IUsuarioRepository userRepository)
    {
        _mapper = mapper;
        _authRepo = authRepository;
        _configuration = configuration;
        _regexSenha = new Regex(@"(?=.*[a-z])(?=.*[0-9])(?=^.{8,50}$).*$");
        _userRepo = userRepository;
    }

    public async Task<EmptyResponse> Register(Usuario user, string password)
    {
        var tupleValidacao = await ValidarRegisterUsuario(user, password);
        if (!tupleValidacao.Item1)
            return new EmptyResponse() { Message = tupleValidacao.Item2 };


        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        GerarEmailConfirmacao(user);

        var usuarioTemporario = await _authRepo.GetUserByCPF(user.CPF);
        if (usuarioTemporario != null)
            user.UsuarioId = usuarioTemporario.UsuarioId;
        
        var userResponse = await _userRepo.CadastrarUsuario(user);
        return new EmptyResponse() { Success = userResponse.UsuarioId > 0 };
    }

    public async Task<ServiceResponse<LoginVMResponse>> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || !Validations.ValidarEmail(email))
            return new ServiceResponse<LoginVMResponse>() { Message = "E-mail inválido :/ Informe novamente, por favor" };

        var usuario = await _authRepo.GetUserByEmail(email);

        if (usuario == null)
            return new ServiceResponse<LoginVMResponse>() { Message = "Não consegui encontrar um cadastro com este E-Mail, caso esteja correto realize o seu cadastro." };
        else if (!VerifyPasswordHash(password, usuario.PasswordHash, usuario.PasswordSalt))
            return new ServiceResponse<LoginVMResponse>() { Message = "Senha inválida, tente informar novamente." };
        else if (!VerifyConfirmacaoUsuario(usuario))
            return new ServiceResponse<LoginVMResponse>() { Message = "Seu cadastro não foi confirmado. Acesse o seu E-mail e clique no link enviado por nós. Caso não encontre verifique a caixa de spam." };

        return new ServiceResponse<LoginVMResponse>() {
            Data = new LoginVMResponse() {
                Token = CreateToken(usuario),
                UsuarioLogado = new UsuarioLogado() {
                    CPF = usuario.CPF,
                    DataNascimento = usuario.DataNascimento,
                    Email = usuario.Email,
                    TipoUsuarioId = usuario.TipoUsuarioId,
                    UsuarioId = usuario.UsuarioId,
                    Nome = usuario.Nome
                }
            },
            Success = true
        };
    }

    public async Task<EmptyResponse> ConfirmarUsuario(string hash)
    {
        if (string.IsNullOrEmpty(hash))
            return new EmptyResponse() { Message = "Hash em branco, tente clicar no link novamente." };

        var confirmarUsuario = await _authRepo.GetConfirmacaoUsuario(hash);

        if (confirmarUsuario == null)
            return new EmptyResponse() { Message = "Não encontrei a sua hash de confirmação." };
        else if (confirmarUsuario.DataConfirmacao != null || confirmarUsuario.EmailConfirmado)
            return new EmptyResponse() { Message = "Usuário já foi confirmado. Tente acessar o sistema." };

        return new EmptyResponse() {
            Message = "Usuário confirmado com sucesso!",
            Success = await _authRepo.ConfirmarUsuario(confirmarUsuario)
        };
    }

    public async Task<EmptyResponse> EsqueciSenha(string email)
    {
        if (string.IsNullOrEmpty(email) || !Validations.ValidarEmail(email))
            return new EmptyResponse() { Message = "E-mail inválido :/ Por favor informe novamente." };
            
        var usuario = await _authRepo.GetUserByEmail(email);
        if (usuario == null)
            return new EmptyResponse() { Message = "Usuário não encontrado." };

        //enviar email
        return new EmptyResponse() {
            Message = "E-mail de recuperação enviado, caso não encontre verifique a caixa de Spam.",
            Success = true
        };
    }

    public async Task<EmptyResponse> EsqueciEmail(string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || !Validations.ValidarCpf(cpf))
            return new EmptyResponse() { Message = "CPF inválido, verifique o cpf digitado." };

        var usuario = await _authRepo.GetUserByCPF(cpf);
        if (usuario == null)
            return new EmptyResponse() { Message = "Usuário não encontrado." };

        //enviar email
        return new EmptyResponse() {
            Message = "E-mail de recuperação para o Email cadastrado, caso não encontre verifique a caixa de Spam.",
            Success = true
        };
    }

    public async Task<ServiceResponse<ClienteVMResponse>> CadastroSimples(Usuario user)
    {
        var tupleValidacao = ValidarCadastroSimples(user);
        if (!tupleValidacao.Item1)
            return new ServiceResponse<ClienteVMResponse>() { Message = tupleValidacao.Item2 };

        return new ServiceResponse<ClienteVMResponse>() { 
            Success = true,
            Data = _mapper.Map<ClienteVMResponse>(await _userRepo.CadastrarUsuario(user))
        };
    }

    public async Task<List<ClienteVMResponse>> GetTop10Clientes(string cpfNome)
    {
        var clientes = await _userRepo.GetTop10Clientes(cpfNome);
        return _mapper.Map<List<ClienteVMResponse>>(clientes);
    }

    private bool VerifyConfirmacaoUsuario(Usuario usuario)
    {
        return usuario.UsuarioConfirmacoesEmail.OrderByDescending(x => x.DataSolicitacao).Select(y => y.EmailConfirmado).FirstOrDefault();
    }

    private async Task<Tuple<bool, string>> ValidarRegisterUsuario(Usuario user, string password)
    {
        if (string.IsNullOrEmpty(user.Nome))
            return new Tuple<bool, string>(false, "Nome inválido. Informe o nome corretamente.");
        if (!Validations.ValidarCpf(user.CPF))
            return new Tuple<bool, string>(false, "CPF inválido. Informe seu CPF corretamente.");
        if (!Validations.ValidarEmail(user.Email))
            return new Tuple<bool, string>(false, "E-mail inválido. Informe seu E-mail corretamente.");

        if (await _authRepo.CPFExists(user.CPF))
            return new Tuple<bool, string>(false, "CPF já cadastrado, tente logar no sistema.");
        if (await _authRepo.EmailExists(user.Email))
            return new Tuple<bool, string>(false, "E-mail já encontrado em nosso banco de dados. Verifique o seu cadastro.");

        if (!_regexSenha.IsMatch(password))
            return new Tuple<bool, string>(false, "A senha deve conter no mínimo 8 caracteres com letras e números.");

        return new Tuple<bool, string>(true, string.Empty);
    }

    private Tuple<bool, string> ValidarCadastroSimples(Usuario user)
    {
        if (string.IsNullOrEmpty(user.Nome))
            return new Tuple<bool, string>(false, "Nome inválido. Informe o nome corretamente.");
        if (!Validations.ValidarCpf(user.CPF))
            return new Tuple<bool, string>(false, "CPF inválido. Informe seu CPF corretamente.");

        return new Tuple<bool, string>(true, string.Empty);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private static void GerarEmailConfirmacao(Usuario user)
    {
        var modelConfirmar = new UsuarioEmailConfirmacao
        {
            DataSolicitacao = DateTime.Now,
            Email = user.Email,
            EmailConfirmado = false,
            UsuarioId = user.UsuarioId
        };

        var key = GetKeyCriptografiaEmailConfirmacao(user.Email, modelConfirmar.DataSolicitacao, user.CPF);

        var hash = GetMd5Hash(key);
        modelConfirmar.Token = hash;

        user.UsuarioConfirmacoesEmail.Add(modelConfirmar);
    }

    private static string GetKeyCriptografiaEmailConfirmacao(string email, DateTime dataSolicitacao, string cpf)
    {
        return email + dataSolicitacao.ToString("dd/MM/yyyyTHH:mm:ss") + string.Join(string.Empty, cpf.Reverse().ToList());
    }

    private static string GetMd5Hash(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }

    static bool VerifyMd5Hash(string input, string hash)
    {
        var hashOfInput = GetMd5Hash(input);
        var comparer = StringComparer.OrdinalIgnoreCase;
        if (0 == comparer.Compare(hashOfInput, hash))
            return true;
        else
            return false;
    }

    private string CreateToken(Usuario user)
    {
        user.PasswordHash = null;
        user.PasswordSalt = null;
        var usuarioLogado = new UsuarioLogado
        {
            UsuarioId = user.UsuarioId,
            TipoUsuarioId = user.TipoUsuarioId,
            CPF = user.CPF,
            DataNascimento = user.DataNascimento,
            Email = user.Email,
            Nome = user.Nome
        };
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UsuarioId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(usuarioLogado))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                    return false;
            }
            return true;
        }
    }
}