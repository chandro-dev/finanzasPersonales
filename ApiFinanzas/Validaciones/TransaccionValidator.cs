using Dominio.DTOS;
using Dominio.Entidades;
using FluentValidation;

namespace ApiFinanzas.Validaciones;


public class TransaccionValidator : AbstractValidator<TransaccionDto>
{
    public TransaccionValidator()
    {
        RuleFor(x => x.Monto)
            .NotEqual(0).WithMessage("El monto no puede ser cero");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción es requerida");

        RuleFor(x => x.Fecha)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha no puede ser en el futuro");

        RuleFor(x => x.CuentaId)
            .GreaterThan(0).WithMessage("CuentaId debe ser mayor a 0");

        RuleFor(x => x.CategoriaId)
            .GreaterThan(0).WithMessage("CategoriaId debe ser mayor a 0");
    }
}
