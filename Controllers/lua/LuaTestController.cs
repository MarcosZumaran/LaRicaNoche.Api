using Microsoft.AspNetCore.Mvc;
using LaRicaNoche.Api.Services.Interfaces.lua;

namespace LaRicaNoche.Api.Controllers.lua;

[ApiController]
[Route("api/[controller]")]
public class LuaTestcontroller : ControllerBase
{
    private readonly ILuaService _luaService;

    // inyeccion de dependencia del core de LUA
    public LuaTestcontroller(ILuaService luaService)
    {
        _luaService = luaService;
    }

    [HttpGet("run")]
    public IActionResult RunLuaTest([FromQuery] string nombre = "Invitado")
    {
        try
        {
            // ejecucion del script
            var resultado = _luaService.ExecuteScriptFile("test.lua", nombre);
            
            return Ok(new
            {
                mensaje = "Lua ha dado una respuesta exitosa.",
                dato_recibido = nombre,
                respuesta_lua = resultado[0]?.ToString()
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Error al ejecutar el script lua", detalle = ex.Message});
        }
    }
}