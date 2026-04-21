namespace LaRicaNoche.Api.Services.Interfaces.lua;

public interface ILuaService
{

    // Ejecuta un script Lua desde un archivo
    object[] ExecuteScriptFile(string fileName, params object[] args);

    // Ejecuta un script Lua desde una cadena de texto
    object[] ExecuteScriptString(string script, params object[] args);
    
}