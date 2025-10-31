using Microsoft.AspNetCore.Identity;

namespace EducaDronAPI.Localization
{
    public class SpanishIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName) =>
            new IdentityError { Code = nameof(DuplicateUserName), Description = $"El nombre de usuario '{userName}' ya está en uso." };

        public override IdentityError DuplicateEmail(string email) =>
            new IdentityError { Code = nameof(DuplicateEmail), Description = $"El correo electrónico '{email}' ya está en uso." };

        public override IdentityError PasswordTooShort(int length) =>
            new IdentityError { Code = nameof(PasswordTooShort), Description = $"La contraseña es demasiado corta. Longitud mínima: {length} caracteres." };

        public override IdentityError PasswordRequiresDigit() =>
            new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "La contraseña debe contener al menos un dígito ('0'-'9')." };

        public override IdentityError PasswordRequiresLower() =>
            new IdentityError { Code = nameof(PasswordRequiresLower), Description = "La contraseña debe contener al menos una letra minúscula ('a'-'z')." };

        public override IdentityError PasswordRequiresUpper() =>
            new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "La contraseña debe contener al menos una letra mayúscula ('A'-'Z')." };

        public override IdentityError PasswordRequiresNonAlphanumeric() =>
            new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "La contraseña debe contener al menos un carácter no alfanumérico." };
    }
}