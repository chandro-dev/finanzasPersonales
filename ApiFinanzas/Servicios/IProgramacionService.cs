namespace ApiFinanzas.Servicios;

public interface IProgramacionService
{
    Task RegistrarProgramacionesActivas();
    Task EjecutarProgramacion(int programacionId);
}
