using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Multi_Request.Data;
using Multi_Request.Dtos;
using Multi_Request.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Multi_Request.Auth
{
    public class UserAuthService:IUserAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UserAuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<UserAuthenticatedDto>> Login(string email, string password)
        {
            var response = new ServiceResponse<UserAuthenticatedDto>();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));

            if (user == null)
            {
                response.Success = false;
                response.Message = "User Does not Exist";
                return response;
            }
            else if (!VerifyPassword(user.Password, password))
            {
                response.Success = false;
                response.Message = "Invalid Password";
                return response;
            }

            var authenticatedUser = new UserAuthenticatedDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = CreateToken(user)
            };
            response.Data = authenticatedUser;
            response.Success = true;
            response.Message = "Successfully Logged In";

            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserRegistrationDto request)
        {
            var response = new ServiceResponse<int>();
            if (await UserExists(request.Email))
            {
                response.Success = false;
                response.Message = "User is Already Exist";
                return response;
            }

            string encryptedPassword = EncryptPassword(request.Password);
            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = encryptedPassword,
                Phone = request.Phone
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            response.Data = newUser.Id;
            response.Message = "Successfully Registered";
            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        private readonly string secretKey = "ThisIsASecretKey123dasfdfsfasfasdfasadf"; // Replace this with a more secure key in a real application

        private byte[] GetValidKey(string key)
        {
            // Pad the key with zeros if it's shorter than 32 bytes
            if (key.Length < 32)
            {
                key = key.PadRight(32, '\0');
            }
            // Truncate the key if it's longer than 32 bytes
            else if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }

            return Encoding.UTF8.GetBytes(key);
        }


        public string EncryptPassword(string password)
        {
            byte[] keyBytes = GetValidKey(secretKey);

            // Use AES encryption to encrypt the password
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.GenerateIV(); // Initialization Vector, a random value for additional security

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encryptedData;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(passwordBytes, 0, passwordBytes.Length);
                    }
                    encryptedData = ms.ToArray();
                }

                // Concatenate IV and encrypted data into a single byte array
                byte[] result = new byte[aes.IV.Length + encryptedData.Length];
                Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
                Array.Copy(encryptedData, 0, result, aes.IV.Length, encryptedData.Length);

                return Convert.ToBase64String(result);
            }
        }

        public bool VerifyPassword(string storedEncryptedPassword, string inputPassword)
        {
            byte[] keyBytes = GetValidKey(secretKey);

            // Use AES decryption to verify the password
            byte[] encryptedPasswordBytes = Convert.FromBase64String(storedEncryptedPassword);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC; // Set the mode to Cipher Block Chaining

                // Extract IV from the first 16 bytes of the encrypted data
                byte[] iv = new byte[16];
                Array.Copy(encryptedPasswordBytes, iv, 16);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] decryptedData;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedPasswordBytes, 16, encryptedPasswordBytes.Length - 16);
                    }
                    decryptedData = ms.ToArray();
                }

                string decryptedPassword = Encoding.UTF8.GetString(decryptedData);
                return inputPassword == decryptedPassword;
            }
        }

        private string CreateToken(User user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:Key"])); ///abcabcabcabcbabbabababababbabababababbaba
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    new Claim(JwtRegisteredClaimNames.Email, user.Email),
    new Claim("DateOfJoing", "31-04-0000"),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};

            // Set the token expiration to one month from the current date
            var expirationDate = DateTime.Now.AddMonths(1);

            var token = new JwtSecurityToken(
                _configuration["JWTToken:Issuer"],
                _configuration["JWTToken:Issuer"],
                claims,
                expires: expirationDate, // Set the expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
