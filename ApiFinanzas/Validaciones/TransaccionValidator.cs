using Dominio.Entidades;
using FluentValidation;

namespace ApiFinanzas.Validaciones;

public class TransaccionValidator : AbstractValidator<Transaccion>
{
    public TransaccionValidator()
    {
        RuleFor(t => t.Monto)
            .NotEqual(0).WithMessage("El monto no puede ser cero.");

        RuleFor(t => t.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(100);

        RuleFor(t => t.CuentaId)
            .GreaterThan(0).WithMessage("Debe seleccionar una cuenta válida.");

        RuleFor(t => t.CategoriaId)
            .GreaterThan(0).WithMessage("Debe seleccionar una categoría válida.");
    }
}
