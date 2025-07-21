using Dominio.DTOS;
using FluentValidation;

namespace ApiFinanzas.Validaciones;

public class CategoriaValidator : AbstractValidator<CategoriaDto>
{
    public CategoriaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");
    }
}
