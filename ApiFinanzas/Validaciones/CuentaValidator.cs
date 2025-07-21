using Dominio.DTOS;
using FluentValidation;

namespace ApiFinanzas.Validaciones;

public class CuentaValidator : AbstractValidator<CuentaDto>
{
    public CuentaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la cuenta es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo de cuenta es obligatorio")
            .Must(t => t == "banco" || t == "efectivo")
            .WithMessage("El tipo de cuenta debe ser 'banco' o 'efectivo'");

        RuleFor(x => x.SaldoInicial)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo");
    }
}
