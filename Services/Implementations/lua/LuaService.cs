using NLua;
using LaRicaNoche.Api.Services.Interfaces.lua;

namespace LaRicaNoche.Api.Services.Implementations.lua;

public class LuaService : ILuaService
{
    private readonly Lua _lua;
    private readonly string _scriptsPath;

    public LuaService(IWebHostEnvironment env)
    {
        _lua = new Lua();

        // Ruta base para los scripts Lua
        _scriptsPath = Path.Combine(env.ContentRootPath, "Scripts");

        // Crear la carpeta de scripts si no existe
        if (!Directory.Exists(_scriptsPath))
        {
            Directory.CreateDirectory(_scriptsPath);
        }
    }

    public object[] ExecuteScriptFile(string fileName, params object[] args)
    {
        string fullPath = Path.Combine(_scriptsPath, fileName);
        if(!File.Exists(fullPath)) throw new FileNotFoundException($"El script lua no existe: {fileName}");

        // Argumentos del script pasados a una variable global en Lua llamada 'args'
        _lua["args"] = args;
        return _lua.DoFile(fullPath);
    }

    public object[] ExecuteScriptString(string script, params object[] args)
    {
        _lua["args"] = args;
        return _lua.DoString(script);
    }

    public void Disponse()
    {
        _lua.Dispose();
    }
}